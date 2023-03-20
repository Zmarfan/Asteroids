using Asteroids.game.asteroids.asteroids;
using Asteroids.game.asteroids.controller;
using Worms.engine.game_object.scripts;

namespace Asteroids.game.asteroids.menu; 

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