using Asteroids.game.asteroids.asteroids;
using Asteroids.game.asteroids.names;
using Asteroids.game.asteroids.shot;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.audio_source;
using Worms.engine.game_object.components.particle_system.ranges;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;

namespace Asteroids.game.asteroids.saucer; 

public class SaucerShooter : Script {
    public delegate void DestroyedSaucerDelegate(bool big);
    public static event DestroyedSaucerDelegate? DestroyedSaucerEvent;
    
    private AudioSource _fireAudioSource = null!;
    private readonly Func<Transform>? _targetSupplier;
    private readonly float _skillRatio;
    private readonly ClockTimer _shootIntervalTimer = new(0.75f);
    private bool _destroyed = false;
    
    public SaucerShooter(Func<Transform>? targetSupplier, float skillRatio) {
        _targetSupplier = targetSupplier;
        _skillRatio = skillRatio;
    }

    public override void Awake() {
        _fireAudioSource = GetComponent<AudioSource>();
    }

    public override void FixedUpdate(float deltaTime) {
        _shootIntervalTimer.Time += deltaTime;
        if (_shootIntervalTimer.Expired()) {
            _fireAudioSource.Restart();
            ShotFactory.Create(Transform.GetRoot(), Transform.Position, GetShootDirection(), 0, false);
            _shootIntervalTimer.Reset();
        }
    }

    private Vector2 GetShootDirection() {
        if (_targetSupplier != null) {
            bool flip = RandomUtil.RandomBool();
            Vector2 toTarget = (_targetSupplier.Invoke().Position - Transform.Position).Normalized;
            Vector2 perpendicular = new(toTarget.y * (flip ? -1 : 1), toTarget.x * (flip ? 1 : -1));
            return Vector2.Lerp(perpendicular, toTarget, RandomUtil.GetRandomValueBetweenTwoValues(_skillRatio, 1));
        }

        return Vector2.InsideUnitCircle();
    }

    public void Die(bool killedByPlayer) {
        if (_destroyed) {
            return;
        }

        if (killedByPlayer) {
            DestroyedSaucerEvent?.Invoke(_targetSupplier == null);
        }
        _destroyed = true;
        ExplosionFactory.CreateExplosion(Transform.GetRoot(), Transform.Position, new RangeZero(10, 20), SoundNames.BANG_MEDIUM);
        Transform.Parent!.gameObject.Destroy();
    }

    public override void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Tag == TagNames.SHOT) {
            Die(true);
            collider.gameObject.Destroy();
        }
    }
}