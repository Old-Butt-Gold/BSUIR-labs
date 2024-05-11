using System.Text;
using Bogus;
using Pastel;
using NAudio;

class Labyrinth
{
    const int ScreenWidth = 160;
    const int ScreenHeight = 62;

    const int MazeHeight = 8;
    const int MazeWidth = 8;

    public const double Depth = 16;
    const double Fov = Math.PI / 3.5;

    public static double PlayerX = 1.5;
    public static double PlayerY = 1.5;
    static double _playerA;

    static StringBuilder _map;
    static readonly int MapHeight = MazeHeight * 3 + 1;
    static readonly int MapWidth = MazeWidth * 3 + 1;

    static bool _isShowMap = true;

    static List<MazeStats> _mazes;
    static int _currentMaze;

    static TaskManager _taskManager = new();
    
    static Faker faker = new();

    static int Score;

    static async Task Main()
    {
        Console.Title = "Labyrinth";
        Console.SetWindowPosition(0, 0);
        Console.SetWindowSize(ScreenWidth, ScreenHeight);
        Console.SetBufferSize(ScreenWidth, ScreenHeight);
        Console.CursorVisible = false;

        InitMap();
        var screen = new char[ScreenWidth * ScreenHeight];
        DateTime GameTime = DateTime.Now;
        DateTime dateTimeFrom = DateTime.Now;
        
        // Создайте объект для воспроизведения аудио
        IWavePlayer waveOutDevice = new WaveOut();
                                                  //new AudioFileReader("1.wav");
        waveOutDevice.Init(new LoopStream(new Mp3FileReader("1.mp3")));
        while (true)
        {
            waveOutDevice.Play();
            DateTime dateTimeTo = DateTime.Now;
            double elapsedTime = (dateTimeTo - dateTimeFrom).TotalSeconds;
            dateTimeFrom = dateTimeTo;
            
            CheckControls(elapsedTime);

            for (int x = 0; x < ScreenWidth; x++)
            {
                double rayAngle = _playerA - Fov / 2 + x * Fov / ScreenWidth;

                double rayX = Math.Cos(rayAngle);
                double rayY = Math.Sin(rayAngle);

                double distanceToWall = 0.0;
                bool hitWall = false;
                bool isBound = false;

                while (!hitWall && distanceToWall < Depth)
                {
                    distanceToWall += 0.1;

                    int testX = (int)(PlayerX + rayX * distanceToWall);
                    int testY = (int)(PlayerY + rayY * distanceToWall);

                    if (testX < 0 || testX >= Depth + PlayerX || testY < 0 || testY >= Depth + PlayerY ||
                        (testX == _mazes[_currentMaze].exitX && testY == _mazes[_currentMaze].exitY))
                    {
                        hitWall = true;
                        distanceToWall = Depth;
                    }
                    else
                    {
                        char testCell = _map[testY * MapWidth + testX];

                        if (testCell == '\u2592')
                        {
                            hitWall = true;

                            distanceToWall *= Math.Cos(rayAngle - _playerA);

                            isBound = Shaders.GetBoundState(testX, testY, rayX, rayY, distanceToWall);
                        }
                    }
                }

                int ceiling = (int)(ScreenHeight / 2.0 - ScreenHeight * Fov / distanceToWall);
                int floor = ScreenHeight - ceiling;

                ceiling += ScreenHeight - ScreenHeight;

                for (int y = 0; y < ScreenHeight; y++)
                {
                    if (y < ceiling)
                        screen[y * ScreenWidth + x] = ' ';
                    else if (y > ceiling && y <= floor)
                        screen[y * ScreenWidth + x] = Shaders.GetWallShade(isBound, distanceToWall);
                    else
                        screen[y * ScreenWidth + x] =
                            Shaders.GetFloorShade(1.0 - (y - ScreenHeight / 2.0) / (ScreenHeight / 2.0));
                }
            }

            //Stats
            char[] stats = $"X: {PlayerX:f5}, Y: {PlayerY:f5}, A: {_playerA:f5}, Score: {Score} Time: {DateTime.Now.AddTicks(-GameTime.Ticks):T} Floor: {5 - _currentMaze} FPS: {(int)(1 / elapsedTime)}".ToCharArray();
            stats.CopyTo(screen, 0);
            
            var mazeInfo = _mazes[_currentMaze];
            if (mazeInfo.exitX == (int) PlayerX && mazeInfo.exitY == (int) PlayerY)
            {
                _currentMaze++;
                var mazeNew = _mazes[_currentMaze];
                UpdateMap(mazeNew.maze, mazeNew.playerX, mazeNew.playerY);
            }
            else if (mazeInfo.entranceX == (int) PlayerX && mazeInfo.entranceY == (int) PlayerY)
            {
                _currentMaze--;
                var mazeNew = _mazes[_currentMaze];
                UpdateMap(mazeNew.maze, mazeNew.playerX, mazeNew.playerY);
            }
            else if (mazeInfo._taskDictionary.ContainsKey(((int) PlayerX, (int) PlayerY)))
            {
                Console.Clear();
                Console.WriteLine("You need to decrypt from morse code");
                var word = mazeInfo._taskDictionary[((int)PlayerX, (int)PlayerY)];
                Console.WriteLine("Morse Code: " + _taskManager.TextToMorseCode(word));
        
                Console.WriteLine("Enter your answer");
                while (word != Console.ReadLine())
                    Console.WriteLine("Incorrect Word");
                mazeInfo._taskDictionary.Remove(((int) PlayerX, (int) PlayerY));
                Score += 5;
            }
            else
            {
                mazeInfo.playerX = PlayerX;
                mazeInfo.playerY = PlayerY;
            }

            if (_isShowMap)
            {
                //Map
                for (int x = 0; x < MapWidth; x++)
                for (int y = 0; y < MapHeight; y++)
                    screen[(y + 1) * ScreenWidth + x] = _map[y * MapWidth + x]; /*' '*/

                //Player
                screen[(int)(PlayerY + 1) * ScreenWidth + (int)PlayerX] = 'P';
            }

            //PlacesWherePlayerWas
            _map[(int)PlayerY * MapWidth + (int)PlayerX] = '\u2665';
            
            Console.SetCursorPosition(0, 0);
            StringBuilder sb = new();
            sb.Append(screen);
            Console.Write(sb.ToString().Pastel(Shaders.GetLevelColor(_currentMaze)));
            //Console.Write(screen);
        }
    }

    static void CheckControls(double elapsedTime)
    {
        if (Console.KeyAvailable)
        {
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.A:
                    _playerA -= elapsedTime * 10;
                    break;
                case ConsoleKey.D:
                    _playerA += elapsedTime * 10;
                    break;
                case ConsoleKey.W:
                {
                    PlayerX += Math.Cos(_playerA) * 30 * elapsedTime;
                    PlayerY += Math.Sin(_playerA) * 30 * elapsedTime;
                    if (_map[(int)PlayerY * MapWidth + (int)PlayerX] == '\u2592')
                    {
                        PlayerX -= Math.Cos(_playerA) * 30 * elapsedTime;
                        PlayerY -= Math.Sin(_playerA) * 30 * elapsedTime;
                    }
                    break;
                }
                case ConsoleKey.S:
                {
                    PlayerX -= Math.Cos(_playerA) * 30 * elapsedTime;
                    PlayerY -= Math.Sin(_playerA) * 30 * elapsedTime;
                    if (_map[(int)PlayerY * MapWidth + (int)PlayerX] == '\u2592')
                    {
                        PlayerX += Math.Cos(_playerA) * 30 * elapsedTime;
                        PlayerY += Math.Sin(_playerA) * 30 * elapsedTime;
                    }
                    break;
                }
                case ConsoleKey.M:
                {
                    _isShowMap = !_isShowMap;
                    break;
                }
            }
        }
    }

    static void InitMap()
    {
        Random random = new();
        _mazes = new(5);
        for (int i = 0; i < 5; i++)
        {
            MazeGenerator mazeGen = new(MapWidth, MapHeight);
            mazeGen.GenerateMaze();
            int entranceX = 0, entranceY = 0;
            int playerX, playerY;
            // Generate random entrance and exit
            if (i != 0)
            {
                entranceY = random.Next(1, MapHeight - 1);
                mazeGen.maze[entranceX, entranceY] = ' ';
            }

            int exitX, exitY;
            do
            {
                exitX = MapWidth - 1;
                exitY = random.Next(1, MapHeight - 1);
            } while (mazeGen.maze[exitX - 1, exitY] == '\u2592');

            mazeGen.maze[exitX, exitY] = ' ';


            // Start with a random player position
            if (i != 0)
                (playerX, playerY) = (entranceX + 1, entranceY);
            else
            {
                do
                {
                    playerX = 1;
                    playerY = random.Next(1, MapHeight - 1);
                } while (mazeGen.maze[playerX, playerY] == '\u2592');
            }
            
            _mazes.Add(new MazeStats(exitX, exitY, entranceX, entranceY, playerX, playerY, mazeGen.maze));
            var maze = _mazes[^1];
            for (int j = 0; j < 4; j++)
            {
                do
                {
                    playerX = random.Next(1, MapWidth - 1);
                    playerY = random.Next(1, MapHeight - 1);
                } while (maze.maze[playerX, playerY] == '\u2592' || 
                         ((int) maze.playerX != playerX && (int) maze.playerY != playerY) || maze._taskDictionary.ContainsKey((playerX, playerY)));

                maze._taskDictionary[(playerX, playerY)] = faker.Random.Word().ToLower();
            }
        }

        var mazeInfo = _mazes[_currentMaze];
        UpdateMap(mazeInfo.maze, mazeInfo.playerX, mazeInfo.playerY);
        
    }

    static void UpdateMap(char[,] map, double playerX, double playerY)
    {
        (PlayerX, PlayerY) = (playerX, playerY);
        _map = new();
        for (int y = 0; y < map.GetLength(1); y++)
            for (int x = 0; x < map.GetLength(0); x++)
                _map.Append(map[x, y]);
    }
}