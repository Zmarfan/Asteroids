﻿using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.particles; 

public class Particles {
    public readonly float duration;
    public readonly bool loop;
    public readonly RangeZero startDelay;
    public readonly RangeZero startLifeTime;
    public readonly RangeZero startSize;
    private readonly RangeZero _startRotation;
    private readonly float _flipRotation;
    public readonly bool playOnAwake;
    public readonly int maxParticles;
    public readonly int seed;
    public readonly StopAction stopAction;

    public Particles(
        float duration, 
        bool loop, 
        RangeZero startDelay, 
        RangeZero startLifeTime, 
        RangeZero startSize, 
        RangeZero startRotation, 
        float flipRotation, 
        bool playOnAwake, 
        int maxParticles, 
        int seed,
        StopAction stopAction
    ) {
        this.duration = Math.Max(duration, 0);
        this.loop = loop;
        this.startDelay = startDelay;
        this.startLifeTime = startLifeTime;
        this.startSize = startSize;
        _startRotation = startRotation;
        _flipRotation = Math.Clamp(flipRotation, 0, 1);
        this.playOnAwake = playOnAwake;
        this.maxParticles = Math.Max(maxParticles, 0);
        this.seed = seed;
        this.stopAction = stopAction;
    }

    public Rotation CalculateInitialRotation(Random random) {
        Rotation rotation = Rotation.FromDegrees(_startRotation.GetRandom(random));
        rotation = random.NextDouble() < _flipRotation ? 360 - rotation : rotation;
        return rotation;
    }
}