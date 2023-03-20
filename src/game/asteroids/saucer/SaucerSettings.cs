using GameEngine.engine.data;
using GameEngine.engine.game_object;

namespace Asteroids.game.asteroids.saucer; 

public readonly struct SaucerSettings {
    public readonly Transform parent;
    public readonly Vector2 position;
    public readonly Func<Transform>? targetSupplier;
    public readonly float skillRatio;

    public SaucerSettings(Transform parent, Vector2 position, Func<Transform>? targetSupplier, float skillRatio) {
        this.parent = parent;
        this.position = position;
        this.targetSupplier = targetSupplier;
        this.skillRatio = skillRatio;
    }
}