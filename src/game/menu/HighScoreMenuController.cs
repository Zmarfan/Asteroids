using Asteroids.game.names;
using GameEngine.engine.core.input;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.scene;

namespace Asteroids.game.menu; 

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