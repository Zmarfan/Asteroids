using Asteroids.game.asteroids.names;
using GameEngine.engine.core.audio;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.animation.animation;
using GameEngine.engine.game_object.components.animation.composition;
using GameEngine.engine.game_object.components.animation.controller;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.helper;

namespace Asteroids.game.asteroids.player; 

public static class PlayerFactory {
    public static Transform Create(Transform parent) {
        Texture playerBase = Texture.CreateMultiple(TextureNames.PLAYER, 0, 0, 1, 2);
        Texture playerThrust = Texture.CreateMultiple(TextureNames.PLAYER, 0, 1, 1, 2);
        
        GameObject obj = parent.AddChild("player")
            .SetLayer(LayerNames.PLAYER)
            .SetTag(TagNames.PLAYER)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateMultiple("player", 0, 0, 1, 2)).Build())
            .SetComponent(new PolygonCollider(false, PlayerBase.COLLIDER_VERTICES, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new PlayerBase())
            .SetComponent(new PlayerMovement())
            .SetComponent(AudioSourceBuilder
                .Builder(SoundNames.THRUST, ChannelNames.EFFECTS)
                .SetName(PlayerMovement.THRUST_AUDIO_SOURCE)
                .SetPlayOnAwake(false)
                .Build()
            )
            .SetComponent(AudioSourceBuilder
                .Builder(SoundNames.FIRE, ChannelNames.EFFECTS)
                .SetName(PlayerMovement.FIRE_AUDIO_SOURCE)
                .SetVolume(Volume.FromPercentage(50))
                .SetPlayOnAwake(false)
                .Build()
            )
            .SetComponent(AnimationControllerBuilder
                .Builder()
                .SetName(PlayerBase.THRUST_ANIMATION_NAME)
                .AddAnimation(PlayerMovement.THRUST_ANIMATION_TRIGGER, new Animation(0.05f, true, ListUtils.Of(
                    new Composition(g => g.GetComponent<TextureRenderer>(), ListUtils.Of(
                        new State(c => ((TextureRenderer)c).texture = playerThrust, 1),
                        new State(c => ((TextureRenderer)c).texture = playerBase, 1)
                    ))
                )))
                .Build()
            )
            .SetComponent(AnimationControllerBuilder
                .Builder()
                .SetName(PlayerBase.START_ANIMATION_NAME)
                .AddAnimation("trigger", new Animation(0.15f, true, ListUtils.Of(
                    new Composition(g => g.GetComponent<TextureRenderer>(), ListUtils.Of(
                        new State(c => ((TextureRenderer)c).Color = Color.TRANSPARENT, 1),
                        new State(c => ((TextureRenderer)c).Color = Color.WHITE, 1)
                    ))
                )))
                .SetStartAnimation(0)
                .Build()
            )
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new BoxCollider(true, ColliderState.TRIGGERING_COLLIDER, new Vector2(80, 80), Vector2.Zero()))
                .Build()
                .Transform.AddSibling("playerKillCollider")
                .SetLayer(LayerNames.PLAYER)
                .SetTag(TagNames.PLAYER)
                .SetComponent(new PolygonCollider(false, PlayerBase.COLLIDER_VERTICES, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        
        Transform.Instantiate(obj);
        return obj.Transform;
    }
}