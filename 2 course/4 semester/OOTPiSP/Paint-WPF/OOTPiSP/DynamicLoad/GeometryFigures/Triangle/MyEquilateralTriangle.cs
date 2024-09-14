using System.Windows.Media;
using SharedComponents;

namespace OOTPiSP.DynamicLoad.GeometryFigures.Triangle;

public class EquilateralMyTriangle : MyTriangle
{
    public override object TagShape => "5";

    public EquilateralMyTriangle(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
        : base(topLeft, downRight, bgColor, penColor, angle)
    {
        CalculateVertexByX(TopLeft, DownRight);
        CalculateVertexByY(TopLeft, DownRight);
    }

    public sealed override void CalculateVertexByX(MyPoint vertex, MyPoint endPoint)
    {
        VertexOX = new(vertex.X + Math.Abs(vertex.X - endPoint.X), vertex.Y);
    }

    public sealed override void CalculateVertexByY(MyPoint vertex, MyPoint endPoint)
    {
        double side = Math.Abs(vertex.X - endPoint.X);
        double height = side * Math.Sqrt(3) / 2;
        VertexOY =  new MyPoint(vertex.X + side / 2, vertex.Y - height);
    }

    public override string ToString() =>
        $"Равносторонний треугольник";
}