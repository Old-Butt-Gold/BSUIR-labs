using System.Windows;
using System.Windows.Media;
using MIAPR_2.Base;

namespace MIAPR_2;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    const int WindowsFrameSize = 30;
    List<Color> Palette { get; }
    Random Random { get; }
    public MainWindow()
    {
        InitializeComponent();
        Palette = new();
        Random = new();
        for (int i = 0; i < 20; i++)
        {
            Color color = Color.FromRgb((byte)Random.Next(256), (byte)Random.Next(256), (byte)Random.Next(256));
            Palette.Add(color);
        }
    }

    async Task Draw(List<ClusterDots> result)
    {
        var drawingGroup = new DrawingGroup();
        for (int i = 0; i < result.Count; i++)
        {
            var item = result[i];
            var ellipses = new GeometryGroup();

            foreach (var point in item.Points)
            {
                var pointSize = point == item.Center ? 10 : 1;
                ellipses.Children.Add(new EllipseGeometry(point, pointSize, pointSize));
            }

            var brush = new SolidColorBrush(Palette[i]);
            var geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 1), ellipses) {Geometry = ellipses};
            drawingGroup.Children.Add(geometryDrawing);
        }
        
        Canvas.Source = new DrawingImage(drawingGroup);
        await Task.Delay(500);
    }
    
    async void ButtonProcessing_OnClick(object sender, RoutedEventArgs e)
    {
        int pointsCount = (int)DotsSlider.Value;
        int width = (int)BorderImage.ActualWidth;
        int height = (int)BorderImage.ActualHeight;

        var points = GenerateRandomPoints(pointsCount, width - WindowsFrameSize, height - WindowsFrameSize);

        var maxiMin = new MaxiMin(points);

        (List<ClusterDots> list, Point? point) result;
        while (true)
        {
            result = maxiMin.Learn();
            if (result.point is null) break;
            await Draw(result.list);
        }

        await Draw(result.list);

        await Task.Delay(1000);

        var kMeans = new KMeans(maxiMin.GetPoints(), maxiMin.GetClusters());
        
        do
        {
            var newResult = kMeans.Learn();
            await Draw(newResult);
        } while (kMeans._isNeedRecalculate);

    }

    List<Point> GenerateRandomPoints(int pointsCount, int width, int height)
    {
        var points = new List<Point>(pointsCount);
        for (int i = 0; i < pointsCount; i++)
            points.Add(new Point(Random.Next(width), Random.Next(height)));
        return points;
    }

}