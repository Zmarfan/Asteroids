using Asteroids.game.asteroids.asteroids;
using Asteroids.game.asteroids.menu;
using Asteroids.game.asteroids.names;
using Asteroids.game.asteroids.player;
using Asteroids.game.asteroids.saucer;
using GameEngine.engine.camera;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.components.rendering.text_renderer;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.helper;
using GameEngine.engine.scene;

namespace Asteroids.game.asteroids.controller; 

public class GameController : Script {
    private static readonly Vector2 SCORE_DISPLAY_OFFSET = new(30, 10);
    private static readonly Vector2 LIFE_DISPLAY_OFFSET = new(40, -90);
    
    private const float MIN_SAUCER_SPAWN_TIME = 10;
    private const float MAX_SAUCER_SPAWN_TIME = 30;

    private Transform _enemyHolder = null!;
    private Transform _lifeDisplayHolder = null!;
    private TextRenderer _scoreTextRenderer = null!;
    private TextRenderer _gameOverTextRenderer = null!;
    private AudioSource _lifeAudioSource = null!;
    private MusicScript _musicScript = null!;
    private ScreenContainer _screenContainer = null!;
    private Transform _player = null!;

    private bool _respawnPlayer = false;
    private bool _waveOver = true;
    private readonly ClockTimer _spawnAsteroidsTimer = new(1.5f, 1.5f);
    private readonly ClockTimer _respawnTimer = new(3);
    private readonly ClockTimer _saucerSpawnerTimer = new(MIN_SAUCER_SPAWN_TIME);
    private long _round = 1;
    private long _score = 0;
    private bool _gameOver = false;
    private readonly ClockTimer _gameOverTimer = new(4f);

    private int Lives {
        get => _lives;
        set {
            foreach (Transform child in _lifeDisplayHolder.children) {
                child.gameObject.Destroy();
            }

            _lives = value;
            LifeFactory.Create(_lifeDisplayHolder, _lives);
        }
    }

    private int _lives;
    
    public GameController() {
        Asteroid.DestroyedAsteroidEvent += AsteroidDestroyedCallback;
        SaucerShooter.DestroyedSaucerEvent += SaucerDestroyedCallback;
        PlayerBase.PlayerDieEvent += PlayerDied;
        _saucerSpawnerTimer.Duration = RandomUtil.GetRandomValueBetweenTwoValues(MIN_SAUCER_SPAWN_TIME, MAX_SAUCER_SPAWN_TIME);
    }

    public override void Awake() {
        _lifeAudioSource = GetComponent<AudioSource>();
        _enemyHolder = Transform.Instantiate(GameObjectBuilder.Builder("enemyHolder")).Transform;
        SetupDisplay();
        Lives = 3;
    }

    public override void Start() {
        _musicScript = Transform.GetRoot().GetComponentInChildren<MusicScript>();
        _screenContainer = Transform.GetRoot().GetComponentInChildren<ScreenContainer>();
        SpawnPlayer();
    }

    public override void Update(float deltaTime) {
        if (_gameOver) {
            _gameOverTimer.Time += deltaTime;
            if (_gameOverTimer.Expired()) {
                EnterHighScore.score = _score;
                SceneManager.LoadScene(ScoreGetsOnTheHighScore() ? SceneNames.ADD_HIGH_SCORE : SceneNames.MAIN_MENU);
            }
            return;
        }
        
        _scoreTextRenderer.Text = _score.ToString();
        
        HandleSaucerSpawning(deltaTime);
        HandlePlayerRespawn(deltaTime);
        if (_enemyHolder.children.Count == 0) {
            HandleWaveSpawn(deltaTime);
        }
    }

    private void HandleSaucerSpawning(float deltaTime) {
        _saucerSpawnerTimer.Time += deltaTime;
        if (_saucerSpawnerTimer.Expired()) {
            _saucerSpawnerTimer.Reset();
            _saucerSpawnerTimer.Duration = RandomUtil.GetRandomValueBetweenTwoValues(MIN_SAUCER_SPAWN_TIME, MAX_SAUCER_SPAWN_TIME);
            bool random = RandomUtil.RandomBool();
            float skillRatio = Math.Min(0.5f + 0.05f * _round, 0.95f);
            SaucerSettings settings = new(_enemyHolder, GetSaucerSpawnPosition(), random ? () => _player : null, skillRatio);
            SaucerFactory.Create(settings);
        }
    }

    private Vector2 GetSaucerSpawnPosition() {
        int side = RandomUtil.RandomBool() ? 1 : -1;
        const float SAUCER_OFFSET = 50;
        float y = RandomUtil.GetRandomValueBetweenTwoValues(-_screenContainer.PlayArea.y / 2f, _screenContainer.PlayArea.y / 2f);
        return new Vector2(side * (_screenContainer.PlayArea.x / 2f) - SAUCER_OFFSET * side, y);
    }

    private void HandlePlayerRespawn(float deltaTime) {
        _respawnTimer.Time += deltaTime;
        if (_respawnPlayer && _respawnTimer.Expired()) {
            _respawnPlayer = false;
            SpawnPlayer();
        }
    }
    
    private void HandleWaveSpawn(float deltaTime) {
        if (!_waveOver) {
            _waveOver = true;
            _spawnAsteroidsTimer.Reset();
        }
        _spawnAsteroidsTimer.Time += deltaTime;
        if (_spawnAsteroidsTimer.Expired()) {
            if (_round % 3 == 0) {
                _lifeAudioSource.Play();
                Lives++;
            }
            SpawnAsteroidWave();
        }
    }
    
    private void SpawnAsteroidWave() {
        _musicScript.RestartMusic();
        _waveOver = false;
        long spawnAmount = Math.Min(2 + _round++, 100);
        for (int i = 0; i < spawnAmount; i++) {
            AsteroidFactory.Create(_enemyHolder, AsteroidType.BIG, _screenContainer.GetRandomPositionAlongBorder());
        }
    }

    private void PlayerDied() {
        Lives--;
        if (Lives != 0) {
            _respawnPlayer = true;
            _respawnTimer.Reset();
            return;
        }

        Asteroid.DestroyedAsteroidEvent -= AsteroidDestroyedCallback;
        SaucerShooter.DestroyedSaucerEvent -= SaucerDestroyedCallback;
        PlayerBase.PlayerDieEvent -= PlayerDied;
        _musicScript.Destroy();
        _gameOverTextRenderer.IsActive = true;
        _gameOver = true;
    }
    
    private void SpawnPlayer() {
        _player = PlayerFactory.Create(Transform.GetRoot());
    }
    
    private void SetupDisplay() {
        _gameOverTextRenderer = GetComponentInChildren<TextRenderer>();
        
        Vector2 displayPosition = new Vector2(
            -WindowManager.CurrentResolution.x / 2f,
            WindowManager.CurrentResolution.y / 2f
        ) * Camera.Main.Size;
        _scoreTextRenderer = TextRendererBuilder
            .Builder(FontNames.MAIN)
            .SetWidth(800)
            .SetSize(25)
            .SetColor(Color.WHITE)
            .Build();
        Transform.Instantiate(GameObjectBuilder
            .Builder("text")
            .SetPosition(displayPosition + SCORE_DISPLAY_OFFSET)
            .SetComponent(_scoreTextRenderer)
        );
        _lifeDisplayHolder = Transform.Instantiate(GameObjectBuilder
            .Builder("lifeHolder")
            .SetPosition(displayPosition + LIFE_DISPLAY_OFFSET)
        ).Transform;
    }
    
    private void AsteroidDestroyedCallback(AsteroidType type) {
        _score += type switch {
            AsteroidType.BIG => 20,
            AsteroidType.MEDIUM => 50,
            AsteroidType.SMALL => 100,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private void SaucerDestroyedCallback(bool big) {
        _score += big ? 250 : 1000;
    }
    
    private bool ScoreGetsOnTheHighScore() {
        List<HighScoreEntry> entries = HighScoreHandler.GetHighScores();
        return entries.Count < HighScoreHandler.MAX_AMOUNT || _score > HighScoreHandler.GetHighScores().Last().score;
    }
}