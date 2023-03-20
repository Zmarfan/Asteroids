using Asteroids.game.asteroids.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.scripts;

namespace Asteroids.game.asteroids.controller; 

public class MusicScript : Script {
    private const float MAX_SPEED = 0.2f;
    private const float MIN_SPEED = 1f;
    
    private readonly ClockTimer _intervalTimer = new(MIN_SPEED);
    private AudioSource _audioSource = null!;
    private bool _interval = true;

    public override void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Update(float deltaTime) {
        _intervalTimer.Duration = Math.Max(_intervalTimer.Duration - 0.01f * deltaTime, MAX_SPEED);
        
        _intervalTimer.Time += deltaTime;
        if (_intervalTimer.Expired()) {
            _intervalTimer.Reset();
            _audioSource.audioId = _interval ? SoundNames.BEAT_1 : SoundNames.BEAT_2;
            _interval = !_interval;
            _audioSource.Restart();
        }
    }

    public void RestartMusic() {
        _intervalTimer.Duration = MIN_SPEED;
    }
}