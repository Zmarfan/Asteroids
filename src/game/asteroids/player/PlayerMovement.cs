using Asteroids.game.asteroids.names;
using Asteroids.game.asteroids.shot;
using GameEngine.engine.core.input;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.animation.controller;
using GameEngine.engine.game_object.components.audio_source;
using GameEngine.engine.game_object.scripts;

namespace Asteroids.game.asteroids.player; 

public class PlayerMovement : Script {
    public const string THRUST_ANIMATION_TRIGGER = "thrust";
    public const string THRUST_AUDIO_SOURCE = "thrust";
    public const string FIRE_AUDIO_SOURCE = "fire";
    
    private const float ROTATE_SPEED = 100;
    private const float THRUST_SPEED = 10;
    private const float MAX_THRUST_SPEED = 125;
    private const float DE_ACCELERATION_FRACTION = 0.975f;
    
    private AnimationController _animationController = null!;
    private AudioSource _thrustAudioSource = null!;
    private AudioSource _fireAudioSource = null!;
    
    private Vector2 _velocity = Vector2.Zero();

    private float _rotateAmount;
    private float _thrust;
    
    private Vector2 ShotSpawnPosition => Transform.Position + Transform.Right * 30;
    
    public override void Awake() {
        _animationController = GetComponents<AnimationController>().First(controller => controller.Name == PlayerBase.THRUST_ANIMATION_NAME);
        List<AudioSource> audioSources = GetComponents<AudioSource>();
        _thrustAudioSource = audioSources.First(source => source.Name == THRUST_AUDIO_SOURCE);
        _fireAudioSource = audioSources.First(source => source.Name == FIRE_AUDIO_SOURCE);

        if (Input.GetButton(InputNames.THRUST)) {
            TurnThrusterEffectsOn();
        }
    }

    public override void Update(float deltaTime) {
        _rotateAmount += Input.GetAxis(InputNames.ROTATE).x;
        
        HandleThrust();

        if (Input.GetButtonDown(InputNames.FIRE)) {
            _fireAudioSource.Restart();
            float initialSpeed = Vector2.Dot(_velocity, Transform.Right);
            ShotFactory.Create(Transform.GetRoot(), ShotSpawnPosition, Transform.Right, initialSpeed * THRUST_SPEED, true);
        }
    }

    public override void FixedUpdate(float deltaTime) {
        Transform.Rotation += _rotateAmount * ROTATE_SPEED * deltaTime;
        _velocity *= DE_ACCELERATION_FRACTION;
        if (_thrust != 0) {
            _velocity += _thrust * Transform.Right;
        }
        if (_velocity.SqrMagnitude >= MAX_THRUST_SPEED * MAX_THRUST_SPEED) {
            _velocity = _velocity.Normalized * MAX_THRUST_SPEED;
        }

        Transform.LocalPosition += _velocity * THRUST_SPEED * deltaTime;
        
        _rotateAmount = 0;
        _thrust = 0;
    }
    private void HandleThrust() {
        _thrust += Input.GetAxis(InputNames.THRUST).x;
        
        if (Input.GetButtonDown(InputNames.THRUST)) {
            TurnThrusterEffectsOn();
        }

        if (Input.GetButtonUp(InputNames.THRUST)) {
            _animationController.Stop();
            _thrustAudioSource.loop = false;
        }
    }

    private void TurnThrusterEffectsOn() {
        _animationController.SetTrigger(THRUST_ANIMATION_TRIGGER);
        _thrustAudioSource.loop = true;
        _thrustAudioSource.Play();
    }
}