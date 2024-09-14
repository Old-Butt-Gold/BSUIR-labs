using System.Windows.Shapes;
using OOTPiSP.DynamicLoad.GeometryFigures;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace OOTPiSP.DynamicLoad.Strategy;

public class LineDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is MyLine myLine)
        {
            Line line = new()
            {
                Fill = myLine.BackgroundColor,
                Stroke = myLine.PenColor,
                X1 = myLine.TopLeft.X,
                X2 = myLine.DownRight.X,
                Y1 = myLine.TopLeft.Y,
                Y2 = myLine.DownRight.Y,
                StrokeThickness = myLine.StrokeThickness
            };

            return line;
        }

        return null;
    }
}