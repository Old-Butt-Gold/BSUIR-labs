using System.Windows.Controls;
using System.Windows.Media;
using MIAPR_8.grammar;

namespace MIAPR_8;

public class Drawer
{
    const int Scale = 10;
    readonly Canvas _canvas;
    const double XStart = 100.0;
    readonly double _yStart;

    public Drawer(Canvas canvas)
    {
        _canvas = canvas;
        _yStart = canvas.Height - 300;
    }

    public void CleanCanvas()
    {
        _canvas.Children.Clear();
        _canvas.Background = Brushes.Black;
    }

    public void Draw(Element element)
    {
        foreach (var line in element.Lines)
        {
            var x1 = GetXCanvasCoordinate(line.From.X);
            var x2 = GetXCanvasCoordinate(line.To.X);
            var y1 = GetYCanvasCoordinate(line.From.Y);
            var y2 = GetYCanvasCoordinate(line.To.Y);

            var lineShape = new System.Windows.Shapes.Line()
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.White,
                Focusable = false
            };
            _canvas.Children.Add(lineShape);
        }
    }

    public void DrawLine(Point from, Point to)
    {
        var x1 = from.X;
        var y1 = from.Y;
        var x2 = to.X;
        var y2 = to.Y;

        var lineShape = new System.Windows.Shapes.Line()
        {
            X1 = x1,
            Y1 = y1,
            X2 = x2,
            Y2 = y2,
            Stroke = Brushes.White,
            Focusable = false,
        };
        _canvas.Children.Add(lineShape);
    }

    public void DrawLine(Line line) => DrawLine(line.From, line.To);

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