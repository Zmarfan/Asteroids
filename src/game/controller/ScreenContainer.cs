using Asteroids.game.names;
using GameEngine.engine.camera;
using GameEngine.engine.core.window;
using GameEngine.engine.data;
using GameEngine.engine.game_object.components.physics.colliders;
using GameEngine.engine.game_object.scripts;
using GameEngine.engine.helper;

namespace Asteroids.game.controller; 

public class ScreenContainer: Script {
    private const float FAR_AWAY = 10000;
    private const float PLAY_AREA_BORDER = 100f;
    
    public Vector2 PlayArea { get; private set; }
    private List<PolygonCollider> _colliders = null!;
    
    public override void Awake() {
        _colliders = GetComponents<PolygonCollider>();
        CalculateScreenArea(WindowManager.CurrentResolution);
    }

    public override void OnTriggerEnter(Collider collider) {
        Vector2 pos = collider.Transform.Parent!.Position;
        List<Vector2> corners = collider.GetLocalCorners()
            .Select(c => collider.Transform.LocalToWorldMatrix.ConvertPoint(c))
            .ToList();
        float maxY = Math.Abs(corners.MaxBy(c => Math.Abs(c.y)).y);
        float maxX = Math.Abs(corners.MaxBy(c => Math.Abs(c.x)).x);
        
        Vector2 half = PlayArea / 2f;
        
        if (maxY > half.y) {
            pos = new Vector2(pos.x, -pos.y + Math.Sign(pos.y) * (maxY - half.y + 10));
        }

        if (maxX > PlayArea.x / 2f) {
            if (collider.gameObject.Tag == TagNames.ENEMY) {
                collider.Transform.Parent!.gameObject.Destroy();
                return;
            }
            
            pos = new Vector2(-pos.x + Math.Sign(pos.x) * (maxX - half.x + 10), pos.y);
        }

        collider.Transform.Parent!.Position = pos;
    }
    
    public Vector2 GetRandomPositionAlongBorder() {
        Vector2 position;
        float p = RandomUtil.GetRandomValueBetweenTwoValues(0, PlayArea.x * 2 + PlayArea.y * 2);
        if (p < PlayArea.x + PlayArea.y) {
            if (p < PlayArea.x) {
                position.x = p;
                position.y = 0;
            }
            else {
                position.x = PlayArea.x;
                position.y = p - PlayArea.x;
            }
        }
        else {
            p -= PlayArea.x + PlayArea.y;
            if (p < PlayArea.x) {
                position.x = PlayArea.x - p;
                position.y = PlayArea.y;
            }
            else {
                position.x = 0;
                position.y = PlayArea.y - (p - PlayArea.x);
            }
        }

        return (position - new Vector2(PlayArea.x, PlayArea.y) / 2f) * 1.5f;
    }
    
    private void CalculateScreenArea(Vector2Int resolution) {
        PlayArea = new Vector2(resolution.x + PLAY_AREA_BORDER, resolution.y + PLAY_AREA_BORDER) * Camera.Main.Size;
        float minX = -PlayArea.x / 2;
        float maxX = PlayArea.x / 2;
        float minY = -PlayArea.y / 2;
        float maxY = PlayArea.y / 2;
        _colliders[0].Vertices = new Vector2[] { new(-FAR_AWAY, minY), new(-FAR_AWAY, maxY), new(minX, maxY), new(minX, minY) };
        _colliders[1].Vertices = new Vector2[] { new(-FAR_AWAY, maxY), new(-FAR_AWAY, FAR_AWAY), new(FAR_AWAY, FAR_AWAY), new(FAR_AWAY, maxY) };
        _colliders[2].Vertices = new Vector2[] { new(maxX, maxY), new(FAR_AWAY, maxY), new(FAR_AWAY, minY), new(maxX, minY) };
        _colliders[3].Vertices = new Vector2[] { new(FAR_AWAY, minY), new(FAR_AWAY, -FAR_AWAY), new(-FAR_AWAY, -FAR_AWAY), new(-FAR_AWAY, minY) };
    }
}