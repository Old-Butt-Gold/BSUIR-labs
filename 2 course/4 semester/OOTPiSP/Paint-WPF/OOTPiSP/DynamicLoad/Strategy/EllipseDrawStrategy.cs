using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using OOTPiSP.DynamicLoad.GeometryFigures.Ellipse;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace OOTPiSP.DynamicLoad.Strategy;

public class EllipseDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is MyEllipse myEllipse)
        {
            double width = myEllipse.GetWidth();
            double height = myEllipse.GetHeight();
        
            System.Windows.Shapes.Ellipse ellipse = new System.Windows.Shapes.Ellipse
            {
                Fill = myEllipse.BackgroundColor,
                Stroke = myEllipse.PenColor,
                Width =  width,
                Height = height,
                StrokeThickness = myEllipse.StrokeThickness,
            };
        
            Canvas.SetLeft(ellipse, myEllipse.TopLeft.X);
            Canvas.SetTop(ellipse, myEllipse.TopLeft.Y);
        
            var CornerOXY = myEllipse.CornerOXY;
            
            if (CornerOXY == 2)
            {
                ellipse.RenderTransform = new RotateTransform(180 + myEllipse.Angle);
            }
        
            if (CornerOXY == 3)
            {
                ellipse.RenderTransform = new RotateTransform(90 + myEllipse.Angle);
            }

            if (CornerOXY == 1)
            {
                ellipse.RenderTransform = new RotateTransform(270 + myEllipse.Angle);
            }

            if (CornerOXY == 4)
            {
                ellipse.RenderTransform = new RotateTransform(myEllipse.Angle);
            }

            if (CornerOXY is 3 or 1)
            {
                (ellipse.Width, ellipse.Height) = (ellipse.Height, ellipse.Width);
            }

            return ellipse;
        }

        return null;
    }
}