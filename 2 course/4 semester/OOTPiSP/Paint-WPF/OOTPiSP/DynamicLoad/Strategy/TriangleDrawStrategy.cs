using System.Windows.Media;
using System.Windows.Shapes;
using OOTPiSP.DynamicLoad.GeometryFigures.Triangle;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace OOTPiSP.DynamicLoad.Strategy;

public class TriangleDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is MyTriangle myTriangle)
        {
            
            double centerX = (myTriangle.TopLeft.X + myTriangle.VertexOX.X + myTriangle.VertexOY.X) / 3;
            double centerY = (myTriangle.TopLeft.Y + myTriangle.VertexOX.Y + myTriangle.VertexOY.Y) / 3;

            Polygon polygon = new()
            {
                Fill = myTriangle.BackgroundColor,
                Stroke = myTriangle.PenColor,
                Points =
                {
                    new System.Windows.Point(myTriangle.TopLeft.X, myTriangle.TopLeft.Y),
                    new System.Windows.Point(myTriangle.VertexOX.X, myTriangle.VertexOX.Y),
                    new System.Windows.Point(myTriangle.VertexOY.X, myTriangle.VertexOY.Y),
                },
                RenderTransform = new RotateTransform(myTriangle.Angle, centerX, centerY),
                StrokeThickness = myTriangle.StrokeThickness,
            };

            return polygon;
        }

        return null;
    }
}