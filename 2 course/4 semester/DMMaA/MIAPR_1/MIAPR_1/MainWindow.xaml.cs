using System.Windows;
using System.Windows.Media;
using MIAPR_1.Base;

namespace MIAPR_1;

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
        await Task.Delay(50);
    }
    
    async void ButtonProcessing_OnClick(object sender, RoutedEventArgs e)
    {
        int pointsCount = (int)DotsSlider.Value;
        int width = (int)BorderImage.ActualWidth;
        int height = (int)BorderImage.ActualHeight;

        var points = GenerateRandomPoints(pointsCount, width - WindowsFrameSize, height - WindowsFrameSize);

        var kMeans = new KMeans(points, (int)ClustersSlider.Value);
        do
        {
            var result = kMeans.Learn();
            await Draw(result);
        } while (kMeans._isNeedRecalculate);
        
    }

    List<Point> GenerateRandomPoints(int pointsCount, int width, int height)
    {
        var points = new List<Point>(pointsCount);
        Parallel.For(0, pointsCount, i =>
        {
            points.Add(new Point(Random.Next(width), Random.Next(height)));
        });
        return points;
    }

}