using GameEngine.engine.data;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.helper;

namespace Asteroids.game.asteroids.saucer; 

public class SaucerMovement : Script {
    private const float VELOCITY = 230f;
    private static readonly List<Vector2> POSSIBLE_DIRECTIONS = ListUtils.Of(
        Vector2.Right(), 
        new Vector2(1, 1).Normalized, 
        new Vector2(1, -1).Normalized
    );

    private readonly ClockTimer _directionChangeTimer = new(2);
    private int _directionIndex = 0;
    private readonly bool _right;
    
    public SaucerMovement(bool right) {
        _right = right;
    }

    public override void FixedUpdate(float deltaTime) {
        _directionChangeTimer.Time += deltaTime;
        if (_directionChangeTimer.Expired()) {
            _directionIndex = RandomUtil.GetRandomZeroToMax(POSSIBLE_DIRECTIONS.Count);
            _directionChangeTimer.Reset();
        }
        
        Transform.Position += POSSIBLE_DIRECTIONS[_directionIndex] * VELOCITY * deltaTime * (_right ? 1 : -1);
    }
}