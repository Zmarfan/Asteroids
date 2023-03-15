﻿using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.player; 

public static class ShotFactory {
    public static void Create(Transform parent, Vector2 position, Vector2 direction, float initialSpeed) {
        GameObject obj = parent.AddChild("shot")
            .SetLayer(LayerNames.SHOT)
            .SetTag(TagNames.SHOT)
            .SetPosition(position)
            .SetComponent(new CircleCollider(true, ColliderState.TRIGGERING_COLLIDER, 7, Vector2.Zero()))
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(TextureNames.SHOT)).Build())
            .SetComponent(new Shot(direction, initialSpeed))
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER, new Vector2(20, 20), Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        Transform.Instantiate(obj);
    }
}