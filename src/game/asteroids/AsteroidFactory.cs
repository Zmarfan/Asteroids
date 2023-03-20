using Asteroids.game.names;
using GameEngine.engine.data;
using GameEngine.engine.game_object;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.components.rendering.texture_renderer;
using GameEngine.engine.helper;

namespace Asteroids.game.asteroids; 

public static class AsteroidFactory {
    private const float MAX_ANGULAR_VELOCITY = 150;

    public static void Create(Transform parent, AsteroidType type, Vector2 position) {
        AsteroidDetails generalDetails = AsteroidDetails.GetDetails(type);
        AsteroidDetails.AsteroidTypeDetails details = generalDetails.details[RandomUtil.GetRandomZeroToMax(generalDetails.details.Count)];
        Vector2 velocity = CalculateInitialVelocity(generalDetails);
        float angularVelocity = RandomUtil.GetRandomValueBetweenTwoValues(-MAX_ANGULAR_VELOCITY, MAX_ANGULAR_VELOCITY);

        GameObject obj = parent.AddChild("asteroid")
            .SetLayer(LayerNames.ASTEROID)
            .SetPosition(position)
            .SetComponent(TextureRendererBuilder.Builder(Texture.CreateSingle(details.textureId)).Build())
            .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGER, Vector2.Zero()))
            .SetComponent(new Asteroid(velocity, angularVelocity, generalDetails))
            .Build()
                .Transform.AddChild("playAreaContainer")
                .SetLayer(LayerNames.PLAY_AREA_OBJECT)
                .SetComponent(new PolygonCollider(true, details.polygonVertices, ColliderState.TRIGGERING_COLLIDER, Vector2.Zero()))
                .Build()
            .Transform.Parent!.gameObject;
        Transform.Instantiate(obj);
    }
    
    private static Vector2 CalculateInitialVelocity(AsteroidDetails details) {
        float speed = RandomUtil.GetRandomValueBetweenTwoValues(details.minVelocity, details.maxVelocity);
        return Vector2.InsideUnitCircle() * speed;
    }
}