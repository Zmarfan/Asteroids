using Asteroids.game.controller;
using Asteroids.game.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.text_renderer;
using GameEngine.engine.scene;

namespace Asteroids.game.scenes; 

public static class GameScene {
    public static Scene GetScene() {
        return new Scene(SceneNames.GAME, CreateWorldRoot, CreateScreenRoot, c => c.Size = 1.5f);
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
            .Build()
            .Transform.AddSibling("gameController")
            .SetComponent(AudioSourceBuilder.Builder(SoundNames.EXTRA_LIFE, ChannelNames.EFFECTS).SetPlayOnAwake(false).Build())
            .SetComponent(new GameController())
            .Build()
            .Transform.AddChild("gameOver")
                .SetLocalPosition(new Vector2(-400, 150))
                .SetComponent(TextRendererBuilder
                    .Builder(FontNames.MAIN)
                    .SetIsActive(false)
                    .SetText("GAME OVER")
                    .SetAlignment(TextAlignment.CENTER)
                    .SetSize(50)
                    .SetWidth(800)
                    .SetColor(Color.WHITE)
                    .Build()
                )
                .Build()
            .Transform.Parent!.AddSibling("music")
            .SetComponent(AudioSourceBuilder.Builder(SoundNames.BEAT_1, ChannelNames.MUSIC).SetPlayOnAwake(false).Build())
            .SetComponent(new MusicScript())
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}