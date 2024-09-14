using System.Windows.Media;
using OOTPiSP.DynamicLoad.Strategy;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.GeometryFigures.Triangle;

public abstract class MyTriangle : AbstractShape
{
    public MyPoint VertexOX { get; set; } 
    public MyPoint VertexOY { get; set; }

    protected MyTriangle(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
        : base(topLeft, downRight, bgColor, penColor, angle)
    {
        DrawStrategy = new TriangleDrawStrategy();
    }

    public abstract void CalculateVertexByX(MyPoint vertex, MyPoint endPoint);
    public abstract void CalculateVertexByY(MyPoint vertex, MyPoint endPoint);

    public override string ToString() =>
        $"{nameof(MyTriangle)}: Vertex1=({TopLeft.X}-{TopLeft.Y}), Vertex2=({VertexOX.X}-{VertexOX.Y}), Vertex3=({VertexOY.X}-{VertexOY.Y})";
}