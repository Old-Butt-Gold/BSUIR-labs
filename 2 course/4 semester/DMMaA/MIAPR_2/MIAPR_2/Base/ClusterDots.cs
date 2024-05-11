using System.Windows;

namespace MIAPR_2.Base;

class ClusterDots
{
    public List<Point> Points { get; set; } = new();
    public Point Center { get; set; }

    public ClusterDots(Point center) => Center = center;

    public Point GetBestClusterCenter()
    {
        var bestCenter = new Point(Points.Average(x => x.X), Points.Average(y => y.Y));
        var minDifferent = double.MaxValue;
        var minDifferentPoint = new Point();
        foreach (var centerCandidate in Points)
        {
            var different = Distance(bestCenter, centerCandidate);
            if (!(different < minDifferent)) continue;
            minDifferent = different;
            minDifferentPoint = centerCandidate;
        }

        return minDifferentPoint;
    }

    public static double Distance(Point first, Point second) =>
        Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
}