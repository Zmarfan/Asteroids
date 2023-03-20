using Asteroids.game.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.components.particle_system;
using GameEngine.engine.game_object.components.particle_system.emission;
using GameEngine.engine.game_object.components.particle_system.particles;
using GameEngine.engine.game_object.components.particle_system.ranges;
using GameEngine.engine.game_object.components.particle_system.renderer;
using GameEngine.engine.game_object.components.particle_system.shape;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using Range = GameEngine.engine.game_object.components.particle_system.ranges.Range;

namespace Asteroids.game.asteroids; 

public static class ExplosionFactory {
    public static void CreateExplosion(Transform parent, Vector2 position, RangeZero particleCount) {
        GameObject obj = CreateExplosionInternal(parent, position, particleCount).Build();
        Transform.Instantiate(obj);
    }
    
    public static void CreateExplosion(Transform parent, Vector2 position, RangeZero particleCount, string audioId) {
        GameObject obj = CreateExplosionInternal(parent, position, particleCount)
            .SetComponent(AudioSourceBuilder.Builder(audioId, ChannelNames.EFFECTS).Build())
            .Build();
        Transform.Instantiate(obj);
    }
    
    public static void CreateShipExplosion(Transform parent, Vector2 position) {
        GameObject obj = parent
            .AddChild("shipExplosion")
            .SetPosition(position)
            .SetComponent(ParticleSystemBuilder
                .Builder(RendererBuilder.Builder(Texture.CreateSingle(TextureNames.SHIP_FRAGMENT)).Build())
                .SetEmission(EmissionBuilder
                    .Builder()
                    .AddBurst(new EmissionBurst(0, new RangeZero(4, 4), 1, 1, 1))
                    .SetRateOverTime(RangeZero.Zero())
                    .Build()
                )
                .SetShape(new Shape(
                    new SphereEmission(25, 1, Rotation.FromDegrees(359)),
                    new VectorRange(new Vector2(5, 5), new Vector2(30, 30))
                ))
                .SetParticles(ParticlesBuilder
                    .Builder()
                    .SetDuration(3f)
                    .SetRotationVelocity(new Range(-50, 50))
                    .SetStartRotation(new RangeZero(0, 359))
                    .SetStartLifeTime(new RangeZero(1.5f, 3f))
                    .SetLoop(false)
                    .SetStopAction(StopAction.DESTROY)
                    .Build()
                )
                .Build()
            )
            .SetComponent(AudioSourceBuilder.Builder(SoundNames.BANG_MEDIUM, ChannelNames.EFFECTS).Build())
            .Build();
        
        Transform.Instantiate(obj);
    }
    
    private static GameObjectBuilder CreateExplosionInternal(Transform parent, Vector2 position, RangeZero particleCount) {
        return parent
            .AddChild("explosion")
            .SetPosition(position)
            .SetComponent(ParticleSystemBuilder
                .Builder(RendererBuilder.Builder(Texture.CreateSingle(TextureNames.FRAGMENT)).Build())
                .SetEmission(EmissionBuilder
                    .Builder()
                    .AddBurst(new EmissionBurst(0, particleCount, 1, 1, 1))
                    .SetRateOverTime(RangeZero.Zero())
                    .Build()
                )
                .SetShape(new Shape(
                    new SphereEmission(1, 1, Rotation.FromDegrees(359)),
                    new VectorRange(new Vector2(30, 30), new Vector2(80, 80))
                ))
                .SetParticles(ParticlesBuilder
                    .Builder()
                    .SetDuration(2f)
                    .SetStartLifeTime(new RangeZero(1f, 2f))
                    .SetLoop(false)
                    .SetStopAction(StopAction.DESTROY)
                    .Build()
                )
                .Build()
            );
    }
}