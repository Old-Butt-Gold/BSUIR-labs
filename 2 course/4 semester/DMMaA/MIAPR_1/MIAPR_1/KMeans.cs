using System.Windows;
using MIAPR_1.Base;

namespace MIAPR_1
{
    class KMeans : Algorithm
    {
        readonly Random _random = new();

        public bool _isNeedRecalculate;

        public KMeans(IEnumerable<Point> points, IEnumerable<ClusterDots> clusters)
        {
            Points = new List<Point>(points);
            Clusters = new List<ClusterDots>(clusters);
        }

        public KMeans(IEnumerable<Point> points, int clustersCount)
        {
            Points = new List<Point>(points);
            Clusters = InitializeClustersWithRandomCenters(clustersCount);
        }

        List<ClusterDots> InitializeClustersWithRandomCenters(int count)
        {
            var result = new List<ClusterDots>();
            var selectedCenters = new List<Point>();
            for (int i = 0; i < count; i++)
            {
                Point point = GetNextRandomCenter();
                selectedCenters.Add(point);
                result.Add(new ClusterDots(point));
            }

            return result;
            
            Point GetNextRandomCenter()
            {
                Point centerCandidate;
                do
                {
                    var index = _random.Next(Points.Count);
                    centerCandidate = Points[index];
                } while (selectedCenters.Contains(centerCandidate));

                return centerCandidate;
            }
        }

        public List<ClusterDots> Learn()
        {
            //do
            //{
                _isNeedRecalculate = false;
                ClearClusters();
                AddPointsToCluster();
                ChangeClusterCenters();
            //} while (_isNeedRecalculate);
            //TODO потом после алгоритма вернуть _isNeedRecalculate to private

            return Clusters;
        }

        void ChangeClusterCenters()
        {
            Parallel.ForEach(Clusters, cluster =>
            {
                var bestCluster = cluster.GetBestClusterCenter();
                if (bestCluster == cluster.Center) return;
                cluster.Center = bestCluster;
                _isNeedRecalculate = true;
            });
        }
    }
}
