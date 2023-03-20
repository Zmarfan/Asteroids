using Asteroids.game.asteroids;
using Worms.engine.core;

namespace Asteroids;

internal static class Program {
    private static void Main() {
        Game game = AsteroidGame.CreateGame();
        game.Run();
    }
}