using System.Windows.Controls;
using System.Windows.Media;

namespace MIAPR_7;

public class Drawer
{
    private readonly Canvas _canvas;
    private const double Scale = 10.0;
    private const double XStart = 50.0;
    private readonly double _yStart;

    public Drawer(Canvas canvas)
    {
        _canvas = canvas;
        _yStart = canvas.Height - 200;
    }

    public void CleanCanvas()
    {
        _canvas.Children.Clear();
        _canvas.Background = Brushes.White;
    }

    public void Draw(Element element)
    {
        var lines = element.Lines;
        foreach (var line in lines)
        {
            var startPoint = new Point(GetXCanvasCoordinate(line.Start.X), GetYCanvasCoordinate(line.Start.Y));
            var endPoint = new Point(GetXCanvasCoordinate(line.End.X), GetYCanvasCoordinate(line.End.Y));
            System.Windows.Shapes.Line finalLine = new()
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = endPoint.X,
                Y2 = endPoint.Y,
                Fill = Brushes.Black,
                Stroke = Brushes.Black,
            };
            _canvas.Children.Add(finalLine);
        }

    }

    public void DrawLine(Point from, Point to)
    {
        System.Windows.Shapes.Line finalLine = new()
        {
            X1 = from.X,
            Y1 = from.Y,
            X2 = to.X,
            Y2 = to.Y,
            Fill = Brushes.Black,
            Stroke = Brushes.Black,
        };
        _canvas.Children.Add(finalLine);
    }

    public void DrawLine(Line line) => DrawLine(line.Start, line.End);

    public Point GetFactPoint(Point canvasPoint)
    {
        var factX = GetXFactCoordinate(canvasPoint.X);
        var factY = GetYFactCoordinate(canvasPoint.Y);
        return new Point(factX, factY);
    }

    private double GetYCanvasCoordinate(double y) => _yStart - y * Scale;

    private double GetXCanvasCoordinate(double x) => x * Scale + XStart;

    private double GetXFactCoordinate(double x) => (x - XStart) / Scale;

    private double GetYFactCoordinate(double y) => (_yStart - y) / Scale;
}