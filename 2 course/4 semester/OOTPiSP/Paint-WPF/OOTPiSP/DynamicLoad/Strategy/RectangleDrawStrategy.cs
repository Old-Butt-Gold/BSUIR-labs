using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OOTPiSP.DynamicLoad.GeometryFigures.Rectangle;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace OOTPiSP.DynamicLoad.Strategy;

public class RectangleDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is MyRectangle myRectangle)
        {
            Rectangle rectangle = new()
            {
                Fill = myRectangle.BackgroundColor,
                Stroke = myRectangle.PenColor,
                Width = myRectangle.GetWidth(),
                Height = myRectangle.GetHeight(),
                StrokeThickness = myRectangle.StrokeThickness,
            };
        
            Canvas.SetLeft(rectangle, myRectangle.TopLeft.X);
            Canvas.SetTop(rectangle, myRectangle.TopLeft.Y);
        
            var CornerOXY = myRectangle.CornerOXY;
            
            if (CornerOXY == 2)
            {
                rectangle.RenderTransform = new RotateTransform(180 + myRectangle.Angle);
            }
        
            if (CornerOXY == 3)
            {
                rectangle.RenderTransform = new RotateTransform(90 + myRectangle.Angle);
            }

            if (CornerOXY == 1)
            {
                rectangle.RenderTransform = new RotateTransform(270 + myRectangle.Angle);
            }

            if (CornerOXY == 4)
            {
                rectangle.RenderTransform = new RotateTransform(myRectangle.Angle);
            }
            
            if (CornerOXY is 3 or 1)
            {
                (rectangle.Width, rectangle.Height) = (rectangle.Height, rectangle.Width);
            }

            return rectangle;

        }

        return null;
    }
}