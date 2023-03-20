using System.Text;
using Asteroids.game.asteroids.names;
using Worms.engine.core.input;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;

namespace Asteroids.game.asteroids.menu; 

public class EnterHighScore : Script {
    public static long score = 0;
    
    private TextRenderer _textRenderer = null!;
    private int _currentIndex = 0;
    private readonly StringBuilder _nickname = new("A__");
    private char _currentChar = 'A';

    public override void Awake() {
        _textRenderer = GetComponent<TextRenderer>();
        _textRenderer.Text = _nickname.ToString();
    }

    public override void Update(float deltaTime) {
        HandlePickingLetter();
        HandleChangingLetter();
    }

    private void HandlePickingLetter() {
        if (Input.GetButtonDown(InputNames.MENU_SELECT)) {
            _currentIndex++;
            if (_currentIndex > 2) {
                SaveHighScore();
            }
            else {
                _currentChar = 'A';
                _nickname[_currentIndex] = _currentChar;
                DisplayText();
            }
        }
    }

    private void HandleChangingLetter() {
        if (Input.GetButtonDown(InputNames.MENU_DOWN)) {
            switch (_currentChar) {
                case 'Z': _currentChar = ' '; break;
                case ' ': _currentChar = 'A'; break;
                default: _currentChar++; break;
            }

            DisplayText();
        }
        else if (Input.GetButtonDown(InputNames.MENU_UP)) {
            switch (_currentChar) {
                case 'A': _currentChar = ' '; break;
                case ' ': _currentChar = 'Z'; break;
                default: _currentChar--; break;
            }
            
            DisplayText();
        }
    }

    private void SaveHighScore() {
        List<HighScoreEntry> entries = HighScoreHandler.GetHighScores();
        if (entries.Count == HighScoreHandler.MAX_AMOUNT) {
            entries = entries.SkipLast(1).ToList();
        }
        entries.Add(new HighScoreEntry(_nickname.ToString(), score));
        entries = entries.OrderByDescending(entry => entry.score).ToList();
        HighScoreHandler.SaveHighScores(entries);
        SceneManager.LoadScene(SceneNames.MAIN_MENU);
    }
    
    private void DisplayText() {
        _nickname[_currentIndex] = _currentChar;
        _textRenderer.Text = _nickname.ToString();
    }
}