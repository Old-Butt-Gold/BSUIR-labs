public static class Shaders
{
    public static char GetWallShade(bool isBound, double distanceToWall)
        {
            char wallShade;
            if (isBound)
                wallShade = '|';
            else if (distanceToWall <= Labyrinth.Depth / 4.0)
                wallShade = '\u2588';
            else if (distanceToWall < Labyrinth.Depth / 3.0)
                wallShade = '\u2593';
            else if (distanceToWall < Labyrinth.Depth / 2.0)
                wallShade = '\u2592';
            else if (distanceToWall < Labyrinth.Depth)
                wallShade = '\u2591';
            else
                wallShade = ' ';
            return wallShade;
        }
        public static char GetFloorShade(double b)
        {
            char floorShade;
            if (b < 0.25)
                floorShade = '#';
            else if (b < 0.5)
                floorShade = 'x';
            else if (b < 0.75)
                floorShade = '-';
            else if (b < 0.9)
                floorShade = '.';
            else
                floorShade = ' ';
            return floorShade;
        }

        public static string GetLevelColor(int number) => number switch
        {
            0 => "#C0C0C0",
            1 => "#BDB76B",
            2 => "#008080",
            3 => "#FF7F50",
            4 => "#FA8072"
        };
        
        public static bool GetBoundState(int testX, int testY, double rayX, double rayY, double distanceToWall)
        {
            var boundsVectorsList = new List<(double module, double cos)>();

            for (int tx = 0; tx < 2; tx++)
            {
                for (int ty = 0; ty < 2; ty++)
                {
                    double vx = testX + tx - Labyrinth.PlayerX;
                    double vy = testY + ty - Labyrinth.PlayerY;

                    double vectorModule = Math.Sqrt(vx * vx + vy * vy);
                    double cosAngle = (rayX * vx / vectorModule) + (rayY * vy / vectorModule);
                    boundsVectorsList.Add((vectorModule, cosAngle));
                }
            }

            boundsVectorsList = boundsVectorsList.OrderBy(v => v.module).ToList();

            double boundAngle = 0.03 / distanceToWall;

            return Math.Acos(boundsVectorsList[0].cos) < boundAngle ||
                   Math.Acos(boundsVectorsList[1].cos) < boundAngle;
        }
}