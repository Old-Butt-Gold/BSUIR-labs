using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace Sun;

public class SunDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is SunShape sun)
        {
            double radius = Math.Min(Math.Abs(sun.TopLeft.X - sun.DownRight.X), Math.Abs(sun.TopLeft.Y - sun.DownRight.Y)) / 2;
            double centerX = (sun.TopLeft.X + sun.DownRight.X) / 2;
            double centerY = (sun.TopLeft.Y + sun.DownRight.Y) / 2;
 
            PathGeometry pathGeometry = new PathGeometry();
 
            EllipseGeometry mainCircle = new EllipseGeometry(new Point(centerX, centerY), radius, radius);
            pathGeometry.AddGeometry(mainCircle);
 
            for (int i = 0; i < 16; i++)
            {
                double angle = i * Math.PI / 8; // 22.5 degrees between each ray
                double rayStartX = centerX + radius * Math.Cos(angle);
                double rayStartY = centerY + radius * Math.Sin(angle);
                double rayEndX = centerX + 2 * radius * Math.Cos(angle);
                double rayEndY = centerY + 2 * radius * Math.Sin(angle);
                LineGeometry ray = new LineGeometry(new Point(rayStartX, rayStartY), new Point(rayEndX, rayEndY));
                pathGeometry.AddGeometry(ray);
            }
 
            Path path = new Path
            {
                Stroke = sun.PenColor,
                Fill = sun.BackgroundColor,
                Data = pathGeometry,
                StrokeThickness = sun.StrokeThickness,
                RenderTransform = new RotateTransform(sun.Angle, centerX, centerY),
            };
 
            return path;
        }

        return null;
    }
}