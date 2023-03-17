﻿using Worms.engine.data;
using Worms.engine.game_object.scripts;

namespace Worms.game.asteroids.shot; 

public class Shot : Script {
    private const float SPEED = 1500;
    private const float LIFE_TIME = 0.75f;

    private readonly Vector2 _direction;
    private readonly float _initialSpeed;
    private readonly ClockTimer _lifeTimer;

    public Shot(Vector2 direction, float initialSpeed) {
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