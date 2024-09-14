using System.Windows.Media;
using SharedComponents;

namespace OOTPiSP.DynamicLoad.GeometryFigures.Triangle;

public class MyIsoscelesTriangle : MyTriangle
{
    public override object TagShape => "6";

    public MyIsoscelesTriangle(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
        : base(topLeft, downRight, bgColor, penColor, angle)
    {
        CalculateVertexByX(TopLeft, DownRight);
        CalculateVertexByY(TopLeft, DownRight);
    }
    
    public sealed override void CalculateVertexByX(MyPoint vertex, MyPoint endPoint)
    {
        VertexOX = new(vertex.X + Math.Abs(endPoint.X - vertex.X), vertex.Y);
    }

    public sealed override void CalculateVertexByY(MyPoint vertex, MyPoint endPoint)
    {
        double sideX = Math.Abs(vertex.X - endPoint.X);
        double sideY = Math.Abs(vertex.Y - endPoint.Y);
        
        double center = sideX / 2;
        double height = sideY;
        VertexOY = new MyPoint(vertex.X + center, vertex.Y - height);
    }

    public override string ToString() =>
        $"равнобедренный треугольник";

}