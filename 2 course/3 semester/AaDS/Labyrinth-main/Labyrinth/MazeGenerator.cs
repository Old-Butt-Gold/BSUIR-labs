class MazeGenerator
{
    public char[,] maze { get; init; }
    int width;
    int height;
    Random random = new();

    public MazeGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
        maze = new char[width, height];
    }

    public void GenerateMaze()
    {
        InitializeMaze();
        CarvePassage(1, 1); //startX, startY
    }

    private void InitializeMaze()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = '\u2592'; // Заполняем стенами
    }

    private void CarvePassage(int x, int y)
    {
        int[] directions = { 1, 2, 3, 4 };
        Shuffle(directions);

        foreach (int direction in directions)
        {
            int dx = 0;
            int dy = 0;

            switch (direction)
            {
                case 1: // Вверх
                    dx = 0;
                    dy = -2;
                    break;
                case 2: // Вправо
                    dx = 2;
                    dy = 0;
                    break;
                case 3: // Вниз
                    dx = 0;
                    dy = 2;
                    break;
                case 4: // Влево
                    dx = -2;
                    dy = 0;
                    break;
            }

            int newX = x + dx;
            int newY = y + dy;

            if (IsInBounds(newX, newY) && maze[newX, newY] == '\u2592')
            {
                maze[x + dx / 2, y + dy / 2] = ' '; // Убираем стену
                maze[newX, newY] = ' '; // Открываем путь
                CarvePassage(newX, newY);
            }
        }
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    private void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int r = i + random.Next(array.Length - i);
            (array[r], array[i]) = (array[i], array[r]);
        }
    }
}