﻿using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.scripts;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.player; 

public class Shot : Script {
    private const float SPEED = 1500;
    private const float LIFE_TIME = 1.5f;

    private readonly Vector2 _direction;
    private readonly float _initialSpeed;
    private readonly ClockTimer _lifeTimer;

    public Shot(Vector2 direction, float initialSpeed) : base(true) {
        _direction = direction;
        _initialSpeed = initialSpeed;
        _lifeTimer = new ClockTimer(LIFE_TIME);
    }

    public override void Update(float deltaTime) {
        _lifeTimer.Time += deltaTime;
        if (_lifeTimer.Expired()) {
            gameObject.Destroy();
        }
    }

    public override void FixedUpdate(float deltaTime) {
        Transform.Position += _direction * (SPEED + _initialSpeed) * deltaTime;
    }
}