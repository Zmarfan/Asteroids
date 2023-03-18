﻿using Worms.engine.core.input;
using Worms.engine.game_object.components.rendering.text_renderer;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.main_menu; 

public class MainMenuController : Script {
    public const string PLAY = "PLAY GAME";
    public const string SCORE = "HIGH SCORES";
    public const string QUIT = "QUIT";
    
    private int _selectedIndex = 0;
    private List<Option> _options = null!;

    public override void Awake() {
        TextRenderer play = GetComponentsInChildren<TextRenderer>().First(r => r.Name == PLAY);
        TextRenderer score = GetComponentsInChildren<TextRenderer>().First(r => r.Name == SCORE);
        TextRenderer quit = GetComponentsInChildren<TextRenderer>().First(r => r.Name == QUIT);
        _options = new List<Option> {
            new(play, () => SceneManager.LoadScene(SceneNames.GAME), PLAY),
            new(score, () => SceneManager.LoadScene(SceneNames.HIGH_SCORE), SCORE),
            new(quit, SceneManager.Quit, QUIT)
        };
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown(InputNames.MENU_SELECT)) {
            _options[_selectedIndex].clickAction.Invoke();
        }

        HandleMenuNavigation();
    }

    private void HandleMenuNavigation() {
        if (Input.GetButtonDown(InputNames.MENU_UP)) {
            _selectedIndex = (_selectedIndex - 1 + _options.Count) % _options.Count;
        }

        if (Input.GetButtonDown(InputNames.MENU_DOWN)) {
            _selectedIndex = (_selectedIndex + 1) % _options.Count;
        }
        
        for (int i = 0; i < _options.Count; i++) {
            _options[i].textRenderer.Text = i == _selectedIndex ? $"- {_options[i].title} -" : _options[i].title;
        }
    }

    protected readonly struct Option {
        public readonly TextRenderer textRenderer;
        public readonly Action clickAction;
        public readonly string title;

        public Option(TextRenderer textRenderer, Action clickAction, string title) {
            this.textRenderer = textRenderer;
            this.clickAction = clickAction;
            this.title = title;
        }
    }
}