﻿using Worms.engine.core.input;
using Worms.engine.game_object.scripts;
using Worms.engine.scene;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.menu; 

public class HighScoreMenuController : Script {
    public override void Start() {
        HighScoreEntryFactory.Create(Transform, HighScoreHandler.GetHighScores());
    }

    public override void Update(float deltaTime) {
        if (Input.GetButtonDown(InputNames.MENU_SELECT)) {
            SceneManager.LoadScene(SceneNames.MAIN_MENU);
        }
    }
}