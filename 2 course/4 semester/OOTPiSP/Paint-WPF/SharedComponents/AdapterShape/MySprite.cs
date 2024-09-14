using System.Windows;
using System.Windows.Media;

namespace SharedComponents.AdapterShape;

public abstract class MySprite
{
    public double _rotationAngle { get; set; }
    [NonSerialized]
    public Point _center;
    public Brush FillColor { get; set; }
    public Brush StrokeColor { get; set; }
    public AbstractDrawStrategy DrawStrategy { get; protected set; }
    public virtual object idOfClassShape { get; }
    public virtual int countOfPoints { get; }
    public double StrokeThickness { get; set; } = 1;
    [NonSerialized]
    public int CanvasIndex = -1;
    public Point[] Points { get; set; }
    public MySprite(Brush fillColor, Brush strokeColor, Point[] points, double rotationAngle)
    {
        FillColor = fillColor;
        StrokeColor = strokeColor;
        Points = points;
        _rotationAngle = rotationAngle;
       
    }
    public PointCollection RotatePoints(Point[] points, double angle)
    {
        double angleInRadians = angle * Math.PI / 180.0;
        PointCollection rotatedPoints = new PointCollection();

        double cosAngle = Math.Cos(angleInRadians);
        double sinAngle = Math.Sin(angleInRadians);

        foreach (var point in points)
        {
            double rotatedX = _center.X + (point.X - _center.X) * cosAngle - (point.Y - _center.Y) * sinAngle;
            double rotatedY = _center.Y + (point.X - _center.X) * sinAngle + (point.Y - _center.Y) * cosAngle;
            rotatedPoints.Add(new Point(rotatedX, rotatedY));
        }

        return rotatedPoints;
    }
    public void CalculateCenter()
    {
        double totalX = 0;
        double totalY = 0;

        foreach (var point in Points)
        {
            totalX += point.X;
            totalY += point.Y;
        }

        _center = new Point(totalX / Points.Length, totalY / Points.Length);
    }
    
    
}