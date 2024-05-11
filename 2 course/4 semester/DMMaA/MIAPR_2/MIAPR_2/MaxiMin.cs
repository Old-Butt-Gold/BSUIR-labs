using System.Runtime.CompilerServices;
using System.Windows;
using MIAPR_2.Base;

namespace MIAPR_2;

class MaxiMin : Algorithm
{
    readonly Random _random = new();

    public MaxiMin(IEnumerable<Point> points)
    {
        Points = new List<Point>(points);
        var firCenter = Points[_random.Next(Points.Count)];
        Clusters = new() { new ClusterDots(firCenter) };
    }

    public List<Point> GetPoints() => Points;
    public List<ClusterDots> GetClusters() => Clusters;

    public (List<ClusterDots> Clusters, Point? Point) Learn()
    {
        //TODO
        /*Point? newCenter;
        do
        {
            ClearClusters();
            AddPointsToCluster();
            newCenter = GetNewCenter();
            if (newCenter is not null)
            {
                Clusters.Add(new(newCenter.Value));
            }
        } while (newCenter != null);

        return Clusters;*/
        
        Point? newCenter;
        ClearClusters();
        AddPointsToCluster();
        newCenter = GetNewCenter();
        if (newCenter is not null)
        {
            Clusters.Add(new(newCenter.Value));
        }

        return (Clusters, newCenter);
    }

    Point? GetNewCenter()
    {
        var averageCenterDistance = GetAverageCenterDistance();
        var newCenterCandidate = GetClustersMaxPoint(Clusters);
        
        return newCenterCandidate.PointDistance > averageCenterDistance / 2 
            ? newCenterCandidate.MaxPoint : null;
    }

    double GetAverageCenterDistance()
    {
        var distanceSum = Clusters
            .SelectMany((cluster, i) => Clusters.Skip(i + 1)
            .Select(otherCluster => ClusterDots.Distance(cluster.Center, otherCluster.Center)))
            .Sum();
        
        /*double distanceSum = 0.0;

        for (int i = 0; i < Clusters.Count; i++)
            for (int j = i + 1; j < Clusters.Count; j++)
                distanceSum = ClusterDots.Distance(Clusters[i].Center, Clusters[j].Center);*/
        
        var count = Enumerable.Range(1, Clusters.Count - 1).Sum();
        return count == 0 ? 0 : distanceSum / count;
    }
    
    ClusterMaxDot GetClustersMaxPoint(IEnumerable<ClusterDots> clusters)
    {
        var maxPoints = new List<ClusterMaxDot>();
        
        foreach (var cluster in clusters)
        {
            double startPointDistance = 0.0;
            Point pointFinal;

            foreach (var point in cluster.Points)
            {
                var pointDistance = ClusterDots.Distance(point, cluster.Center);
                if (pointDistance > startPointDistance)
                {
                    startPointDistance = pointDistance;
                    pointFinal = point;
                }
            }

            maxPoints.Add(new () {MaxPoint = pointFinal, PointDistance = startPointDistance});
        }

        return maxPoints.MaxBy(x => x.PointDistance)!;
    }


    class ClusterMaxDot
    {
        public double PointDistance { get; set; }

        public Point MaxPoint { get; set; }
    }
}
