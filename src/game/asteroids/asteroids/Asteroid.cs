﻿using Worms.engine.core.cursor;
using Worms.engine.core.input;
using Worms.engine.core.input.listener;
using Worms.engine.data;
using Worms.engine.game_object;
using Worms.engine.game_object.components.physics;
using Worms.engine.game_object.components.physics.colliders;
using Worms.engine.game_object.components.rendering.texture_renderer;
using Worms.engine.game_object.scripts;
using Worms.engine.helper;
using Worms.game.asteroids.names;

namespace Worms.game.asteroids.asteroids; 

public class Asteroid : Script {
    private const float MAX_ANGULAR_VELOCITY = 100;
    
    private static readonly Random RANDOM = new();
    
    private readonly Vector2 _velocity;
    private readonly float _angularVelocity;
    
    private Asteroid(Vector2 velocity, float angularVelocity) : base(true) {
        _velocity = velocity;
        _angularVelocity = angularVelocity;
    }

    public static GameObject Create(Transform parent, AsteroidType type, Vector2 position) {
        AsteroidDetails generalDetails = AsteroidDetails.GetDetails(type);
        AsteroidDetails.AsteroidTypeDetails details = generalDetails.details[RANDOM.Next(generalDetails.details.Count)];
        Vector2 velocity = CalculateInitialVelocity(generalDetails);
        float angularVelocity = RandomUtil.GetRandomValueBetweenTwoValues(RANDOM, -MAX_ANGULAR_VELOCITY, MAX_ANGULAR_VELOCITY);

        return parent.AddChild("asteroid1")
            .SetLayer(LayerNames.ASTEROID)
            .SetPosition(position)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(details.textureId)).Build())
            .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new Asteroid(velocity, angularVelocity))
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
    }

    public override void Update(float deltaTime) {
        Transform.Position += _velocity * deltaTime;
        Transform.Rotation += _angularVelocity * deltaTime;
        
        
        if (Input.GetKeyDown(Button.LEFT_MOUSE)) {
            Console.WriteLine(Transform.WorldToLocalMatrix.ConvertPoint(Input.MouseWorldPosition));
        }
    }

    public override void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.Tag == TagNames.SHOT) {
            collider.gameObject.Destroy();
        }
    }
    
    private static Vector2 CalculateInitialVelocity(AsteroidDetails details) {
        float speed = RandomUtil.GetRandomValueBetweenTwoValues(RANDOM, details.minVelocity, details.maxVelocity);
        return Vector2.InsideUnitCircle(RANDOM) * speed;
    }
}