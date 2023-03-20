using Asteroids.game.asteroids;
using Asteroids.game.controller;
using GameEngine.engine.game_object.scripts;

namespace Asteroids.game.menu; 

public class MenuAsteroidsSpawner : Script {
    private ScreenContainer _screenContainer = null!;
    
    public override void Start() {
        _screenContainer = GetComponent<ScreenContainer>();
        
        for (int i = 0; i < 4; i++) {
            AsteroidFactory.Create(Transform.GetRoot(), AsteroidType.BIG, _screenContainer.GetRandomPositionAlongBorder());
            AsteroidFactory.Create(Transform.GetRoot(), AsteroidType.MEDIUM, _screenContainer.GetRandomPositionAlongBorder());
            AsteroidFactory.Create(Transform.GetRoot(), AsteroidType.SMALL, _screenContainer.GetRandomPositionAlongBorder());
        }
    }
}