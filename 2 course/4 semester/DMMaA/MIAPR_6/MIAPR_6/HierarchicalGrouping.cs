using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;

namespace MIAPR_6;

public class HierarchicalGrouping
{
    readonly List<Group> _groups = new();
    public static int DistanceX { get; set; }
    int _numberOfChar;

    public HierarchicalGrouping(double[,] distances, int size)
    {
        DistanceX = 10;
        for (int i = 0; i < size; i++)
        {
            _groups.Add(new Group { Name = "X" + (i + 1) });
        }
        for (int i = 0; i < _groups.Count; i++)
        {
            for (int j = 0; j < _groups.Count; j++)
            {
                if (i != j)
                {
                    _groups[i].Distances.Add(new Distances(distances[i, j], _groups[j]));
                }
            }
        }
    }

    char NextChar()
    {
        const string alphabet = "ABCDEFGHIKLMNOPQRSTVXYZ";
        _numberOfChar = _numberOfChar == alphabet.Length - 1 ? 0 : _numberOfChar + 1;
        return alphabet[_numberOfChar - 1];
    }

    void AddGroups(List<Group> addedGroups, double minDistance)
    {
        var newGroup = new Group
        {
            Name = NextChar().ToString()
        };
            
        foreach (Group group in _groups)
        {
            if (!addedGroups.Contains(group))
            {
                double minDist = group.GetDistance(addedGroups[0]);
                foreach (Group currGroup in addedGroups)
                {
                    var currentDist = group.GetDistance(currGroup);
                    if (currentDist < minDist)
                    {
                        minDist = currentDist;
                    }
                }
                group.DeleteDistances(addedGroups);
                group.Distances.Add(new Distances(minDist, newGroup));
                newGroup.Distances.Add(new Distances(minDist, group));
            }
        }
        foreach (Group addedGroup in addedGroups)
        {
            if (addedGroup.X == 0)
            {
                addedGroup.X = DistanceX;
                DistanceX += 10;
            }
        }
        newGroup.SubGroups = addedGroups;
        double xCoord = 0;
        foreach (Group addedGroup in addedGroups)
        {
            xCoord += addedGroup.X;
            _groups.Remove(addedGroup);
        }
        
        newGroup.X = xCoord / addedGroups.Count;
        newGroup.Y = minDistance;
        _groups.Add(newGroup);
    }

    public void FindGroups()
    {
        bool result = false;
        do
        {
            result = false;
            var groupWithMinDist = new List<Group>();
            double minDistance = double.MaxValue;
            foreach (Group group in _groups)
            {
                foreach (Distances distance in group.Distances)
                {
                    if (distance.Distance <= minDistance)
                    {
                        if (distance.Distance < minDistance)
                        {
                            minDistance = distance.Distance;
                            result = true;
                            groupWithMinDist.Clear();
                        }
                        groupWithMinDist.Add(group);
                    }
                }
            }
            if (result && groupWithMinDist.Count > 1)
            {
                AddGroups(groupWithMinDist, minDistance);
            }
        } while (result);
    }

    public List<Group> GetGroups() => _groups;
}

public class ChartDrawer
{
    readonly List<Color> _colors = new() {
        Color.Red,
        Color.Blue,
        Color.DodgerBlue,
        Color.DarkMagenta,
        Color.BlueViolet,
        Color.DeepPink,
        Color.Firebrick,
        Color.ForestGreen,
        Color.MidnightBlue,
        Color.Green,
        Color.Aqua,
        Color.DarkOrchid,
        Color.RoyalBlue,
        Color.DarkBlue,
        Color.Beige,
        Color.Salmon,
        Color.Sienna,
        Color.PowderBlue,
        Color.Plum,
        Color.LightSalmon,
        Color.DarkOrchid,
        Color.Olive,
        Color.YellowGreen,
        Color.Violet,
        Color.Gold,
        Color.Yellow,
        Color.LightCyan,
        Color.LightBlue,
        Color.Turquoise
    };
    
    int _nextColor;
    
    void SetUpChart(Chart chart, List<Group> groups)
    {
        
        chart.Series.Clear();
            
        chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
        chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            
        chart.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.Lines;
        chart.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.Lines;
            
        chart.ChartAreas[0].AxisX.IsStartedFromZero = true;
        chart.ChartAreas[0].AxisY.IsStartedFromZero = true;
            
        chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
        chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            
        chart.ChartAreas[0].AxisX.Minimum = 0;
        chart.ChartAreas[0].AxisY.Minimum = 0;
            
        chart.ChartAreas[0].AxisX.Maximum = HierarchicalGrouping.DistanceX;
        chart.ChartAreas[0].AxisY.Maximum = groups[0].Y + 0.01;
    }

    void DrawSubGroups(Group group, Chart chart)
    {
        bool res = true;
        foreach (Series currSeries in chart.Series)
        {
            if (currSeries.Name == group.Name)
            {
                res = false;
            }
        }
        
        if (res)
        {
            var pointsSeries = new Series {ChartType = SeriesChartType.Point};
            pointsSeries.Name = group.SubGroups.Count != 0 
                ? $"Группа: {group.Name}" : $"Точка: {group.Name}";

            pointsSeries.MarkerSize = 30;
            pointsSeries.MarkerColor = _colors[_nextColor];
            _nextColor = _nextColor == 28 ? 0 : _nextColor + 1;
                
            pointsSeries.Points.AddXY(group.X, group.Y);
            if (chart.Series.IndexOf(pointsSeries) == -1)
            {
                chart.Series.Add(pointsSeries);
            }
            
            foreach (Group subGroup in group.SubGroups)
            {
                var lineSeries = new Series {ChartType = SeriesChartType.Line};
                lineSeries.BorderWidth = 4;
                lineSeries.Name = group.Name + "<->" + subGroup.Name;
                lineSeries.IsVisibleInLegend = false; //!!!
                lineSeries.Color = pointsSeries.MarkerColor;
                lineSeries.Points.AddXY(subGroup.X, subGroup.Y);
                lineSeries.Points.AddXY(subGroup.X, group.Y);
                lineSeries.Points.AddXY(group.X, group.Y);
                res = true;
                
                foreach (Series currSeries in chart.Series)
                {
                    if (currSeries.Name == lineSeries.Name)
                    {
                        res = false;
                    }
                }
                
                if (res)
                {
                    chart.Series.Add(lineSeries);
                }
                DrawSubGroups(subGroup, chart);
            }
        }
    }

    public void Draw(Chart chart, List<Group> groups)
    {
        SetUpChart(chart, groups);
        foreach (Group subGroup in groups)
        {
            DrawSubGroups(subGroup, chart);
        }
    }
}

public class Distances
{
    public readonly double Distance;
    public readonly Group Group;
    public override string ToString() => $"Name: {Group.Name}; Distance: {Distance};";

    public Distances(double dist, Group sub) => (Distance, Group) = (dist, sub);
}

public class Group
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Name: {Name} ");
        foreach (var item in Distances)
        {
            sb.Append($"{item.Group.Name}:{item.Distance}; ");
        }

        return sb.ToString();
    }

    public readonly List<Distances> Distances = new();
    public string Name;
    public List<Group> SubGroups = new();
    public double X;
    public double Y;

    public double GetDistance(Group group)
    {
        foreach (Distances distance in Distances)
        {
            if (distance.Group.Equals(group))
            {
                return distance.Distance;
            }
        }
        return -1;
    }

    public void DeleteDistances(List<Group> deleteList)
    {
        foreach (Group deleteGroup in deleteList)
        {
            var deleteDistances = new List<Distances>();
            foreach (Distances distance in Distances)
            {
                if (distance.Group.Equals(deleteGroup))
                {
                    deleteDistances.Add(distance);
                }
            }
            foreach (Distances deleteDistance in deleteDistances)
            {
                Distances.Remove(deleteDistance);
            }
        }
    }
}