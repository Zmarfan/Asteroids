using Asteroids.game.names;
using GameEngine.engine.core.audio;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.texture_renderer;

namespace Asteroids.game.saucer; 

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
            .SetScale(settings.targetSupplier == null ? Vector2.One() : new Vector2(0.75f, 0.75f))
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(TextureNames.ENEMY)).Build())
            .SetComponent(new PolygonCollider(true, COLLIDER_VERTICES, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
            .SetComponent(new SaucerMovement(settings.position.x < 0))
            .SetComponent(AudioSourceBuilder
                .Builder(settings.targetSupplier == null ? SoundNames.SAUCER_BIG : SoundNames.SAUCER_SMALL, ChannelNames.EFFECTS)
                .SetVolume(Volume.FromPercentage(50))
                .SetLoop(true)
                .Build()
            )
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetTag(TagNames.ENEMY)
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER, new Vector2(70, 50), Vector2.Zero()))
                .Build()
                .Transform.AddSibling("enemyHitZone")
                .SetTag(TagNames.ENEMY)
                .SetLayer(LayerNames.ENEMY)
                .SetComponent(new SaucerShooter(settings.targetSupplier, settings.skillRatio))
                .SetComponent(AudioSourceBuilder.Builder(SoundNames.FIRE, ChannelNames.EFFECTS).SetPlayOnAwake(false).SetVolume(Volume.FromPercentage(50)).Build())
                .SetComponent(new PolygonCollider(true, COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        
        Transform.Instantiate(obj);
    }
}