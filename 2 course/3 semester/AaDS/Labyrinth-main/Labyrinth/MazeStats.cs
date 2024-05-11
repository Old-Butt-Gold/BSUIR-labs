public class MazeStats
{
    public int exitX, exitY;
    public int entranceX, entranceY;
    public double playerX, playerY;
    public char[,] maze;
    public Dictionary<(int TaskX, int TaskY), string> _taskDictionary = new(4);

    public MazeStats(int exitX, int exitY, int entranceX, int entranceY, double playerX, double playerY, char[,] maze)
    {
        this.exitX = exitX;
        this.exitY = exitY;
        this.entranceX = entranceX;
        this.entranceY = entranceY;
        this.playerX = playerX;
        this.playerY = playerY;
        this.maze = maze;
    }
}