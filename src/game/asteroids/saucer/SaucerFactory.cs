﻿using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.saucer; 

public static class SaucerFactory {
    private static readonly Vector2[] COLLIDER_VERTICES = {
        new (-14, -23),
        new (-28, -10),
        new (-35, -10),
        new (-35, -2),
        new (-14, 10),
        new (-8, 24),
        new (8, 24),
        new (14, 10),
        new (35, -2),
        new (35, -10),
        new (28, -10),
        new (14, -23),
    };
    
    public static void Create(SaucerSettings settings) {
        GameObject obj = settings.parent.AddChild("saucer")
            .SetLayer(LayerNames.ENEMY)
            .SetTag(TagNames.ENEMY)
            .SetPosition(settings.position)
            .SetScale(settings.target == null ? Vector2.One() : new Vector2(0.75f, 0.75f))
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(TextureNames.ENEMY)).Build())
            .SetComponent(new PolygonCollider(true, COLLIDER_VERTICES, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
            .SetComponent(new SaucerMovement(settings.goesToTheRight))
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetTag(TagNames.ENEMY)
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER, new Vector2(70, 50), Vector2.Zero()))
                .Build()
                .Transform.AddSibling("enemyHitZone")
                .SetTag(TagNames.ENEMY)
                .SetLayer(LayerNames.ENEMY)
                .SetComponent(new SaucerShooter(settings.target, settings.skillRatio))
                .SetComponent(AudioSourceBuilder.Builder(SoundNames.FIRE, ChannelNames.EFFECTS).Build())
                .SetComponent(new PolygonCollider(true, COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        
        Transform.Instantiate(obj);
    }
}