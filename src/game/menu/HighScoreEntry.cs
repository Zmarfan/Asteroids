namespace Asteroids.game.menu; 

public readonly struct HighScoreEntry {
    public readonly string name;
    public readonly long score;

    public HighScoreEntry(string name, long score) {
        this.name = name;
        this.score = score;
    }
}