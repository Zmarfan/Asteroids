﻿using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.scene;
using Worms.game.asteroids.controller;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.scenes; 

public static class GameScene {
    public static Scene GetScene() {
        return new Scene("main", CreateWorldRoot, CreateScreenRoot);
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
            .Transform.AddSibling("music")
            .SetComponent(AudioSourceBuilder.Builder(SoundNames.BEAT_1, ChannelNames.MUSIC).SetPlayOnAwake(false).Build())
            .SetComponent(new MusicScript())
            .Build()
            .Transform.GetRoot().gameObject;
    }

    private static GameObject CreateScreenRoot() {
        return GameObjectBuilder.Root();
    }
}