﻿using Worms.engine.data;

namespace Worms.engine.game_object.components.particle_system.shape; 

public class LineEmission : IEmissionShape {
    private readonly float _radius;

    public LineEmission(float radius) {
        _radius = radius;
    }

    public Tuple<Vector2, Vector2> GetSpawnPositionAndDirection(Random random) {
        return new Tuple<Vector2, Vector2>(new Vector2((float)(random.NextDouble() * _radius), 0), Vector2.Up());
    }
}