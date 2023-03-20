using Asteroids.game.asteroids.controller;
using Asteroids.game.asteroids.menu;
using Asteroids.game.asteroids.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.text_renderer;
using GameEngine.engine.scene;

namespace Asteroids.game.asteroids.scenes; 

public static class HighScoreScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.HIGH_SCORE, CreateWorldRoot, CreateScreenRoot, c => c.Size = 1.5f);
    }
    
    private static GameObject CreateWorldRoot() {
        Vector2[] defaultVertices = { new(-1, -1), new(-1, 1), new(1, 1) };
        
        return GameObjectBuilder.Root()
            .Transform.AddChild("screenContainer")
            .SetLayer(LayerNames.PLAY_AREA_OBJECT)
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PolygonCollider(true, defaultVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new ScreenContainer())
            .SetComponent(new MenuAsteroidsSpawner())
            .Build()
            .Transform.AddSibling("highscore")
            .SetComponent(new HighScoreMenuController())
            .SetPosition(new Vector2(-400, 450))
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(45)
                .SetText("HIGH SCORES")
                .Build()
            )
            .Build()
            .Transform.AddSibling("back")
            .SetPosition(new Vector2(-400, -350))
            .SetComponent(TextRendererBuilder
                .Builder(FontNames.MAIN)
                .SetAlignment(TextAlignment.CENTER)
                .SetWidth(800)
                .SetColor(Color.WHITE)
                .SetSize(25)
                .SetText("- BACK -")
                .Build()
            )
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}