using Asteroids.game.asteroids;
using Asteroids.game.names;
using Asteroids.game.saucer;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.animation.controller;
using GameEngine.engine.game_object.components.particle_system.ranges;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.scripts;

namespace Asteroids.game.player; 

public class PlayerBase : Script {
    public const string START_ANIMATION_NAME = "startAnimation";
    public const string THRUST_ANIMATION_NAME = "thrustAnimation";

    public delegate void PlayerDieDelegate();
    public static event PlayerDieDelegate? PlayerDieEvent;
    
    public static readonly Vector2[] COLLIDER_VERTICES = { new(-15, 12), new(25, 0), new(-15, -12) };
    
    private bool _dead = false;
    private readonly ClockTimer _canDieTimer = new(5);
    private bool _canDie = false;
    private AnimationController _animationController = null!;

    public override void Awake() {
        _animationController = GetComponents<AnimationController>().First(controller => controller.Name == START_ANIMATION_NAME);
    }

    public override void Update(float deltaTime) {
        _canDieTimer.Time += deltaTime;
        if (!_canDie && _canDieTimer.Expired()) {
            _canDie = true;
            _animationController.Stop();
            GetComponentsInChildren<PolygonCollider>().ForEach(collider => collider.IsActive = true);
        }
    }

    public override void OnTriggerEnter(Collider collider) {
        if (_dead) {
            return;
        }

        _dead = true;
        
        switch (collider.gameObject.Tag) {
            case TagNames.SHOT:
                collider.gameObject.Destroy();
                break;
            case TagNames.ENEMY:
                collider.GetComponentInChildren<SaucerShooter>().Die(true);
                break;
        }
        Die();
    }
    
    public void Die() {
        ExplosionFactory.CreateShipExplosion(Transform.GetRoot(), Transform.Position);
        ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, new RangeZero(5, 15));
        PlayerDieEvent?.Invoke();
        gameObject.Destroy();
    }
}