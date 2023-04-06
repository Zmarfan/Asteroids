using Asteroids.game;
using GameEngine.engine.core;

namespace Asteroids;

internal static class Program {
    private static void Main() {
        Game game = AsteroidGame.CreateGame();
        game.Run();
    }
}