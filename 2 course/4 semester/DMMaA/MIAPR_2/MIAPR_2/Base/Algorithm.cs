using System.Windows;

namespace MIAPR_2.Base;

abstract class Algorithm
{
    protected List<Point> Points;
    protected List<ClusterDots> Clusters;

    protected void ClearClusters()
    {
        Clusters.AsParallel().ForAll(x => x.Points = new(){x.Center});
    }
    
    protected void AddPointsToCluster()
    {
        foreach (var point in Points)
        {
            var minDifferentCluster = GetMinDifferentCluster(point);
            minDifferentCluster?.Points.Add(point);
        }
    }

    ClusterDots? GetMinDifferentCluster(Point point)
    {
        var minDifferent = double.MaxValue;
        ClusterDots? minDifferentCluster = null;
        foreach (var pointCluster in Clusters)
        {
            if (point == pointCluster.Center) return null;
            var different = ClusterDots.Distance(pointCluster.Center, point);
            if (different < minDifferent)
            {
                minDifferent = different;
                minDifferentCluster = pointCluster;
            }
        }

        return minDifferentCluster;
    }
}