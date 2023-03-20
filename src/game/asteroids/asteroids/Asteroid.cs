using Asteroids.game.asteroids.names;
using Asteroids.game.asteroids.player;
using Asteroids.game.asteroids.saucer;
using Worms.engine.data;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;

namespace Asteroids.game.asteroids.asteroids; 

public class Asteroid : Script {
    public delegate void DestroyedAsteroidDelegate(AsteroidType type);
    public static event DestroyedAsteroidDelegate? DestroyedAsteroidEvent;
    
    private readonly Vector2 _velocity;
    private readonly float _angularVelocity;
    private readonly AsteroidDetails _details;
    private bool _destroyed = false;
    
    public Asteroid(Vector2 velocity, float angularVelocity, AsteroidDetails details) {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
        _details = details;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _velocity * deltaTime;
        Transform.Rotation += _angularVelocity * deltaTime;
    }

    public override void OnTriggerEnter(Collider collider) {
        if (_destroyed || collider.gameObject.Tag is not (TagNames.SHOT or TagNames.ENEMY or TagNames.PLAYER)) {
            return;
        }

        _destroyed = true;
        switch (collider.gameObject.Tag) {
            case TagNames.ENEMY:
                collider.GetComponentInChildren<SaucerShooter>().Die(false);
                ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, _details.particleCount);
                break;
            case TagNames.PLAYER:
                collider.Transform.Parent!.GetComponent<PlayerBase>().Die();
                ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, _details.particleCount);
                break;
            default:
                collider.gameObject.Destroy();
                ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, _details.particleCount, _details.explosionAudioId);
                break;
        }
            
        if (_details.type != AsteroidType.SMALL) {
            SpawnNewAsteroids();
        }
        gameObject.Destroy();
        DestroyedAsteroidEvent?.Invoke(_details.type);
    }

    private void SpawnNewAsteroids() {
        AsteroidType newType = _details.type == AsteroidType.BIG ? AsteroidType.MEDIUM : AsteroidType.SMALL;
        AsteroidFactory.Create(Transform.Parent!, newType, Transform.Position);
        AsteroidFactory.Create(Transform.Parent!, newType, Transform.Position);
    }
}