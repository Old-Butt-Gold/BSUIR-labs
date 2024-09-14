using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace Star;

public class StarDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is StarShape star)
        {
            double centerX = (star.TopLeft.X + star.DownRight.X) / 2;
            double centerY = (star.TopLeft.Y + star.DownRight.Y) / 2;
            double outerRadius = Math.Min(Math.Abs(star.TopLeft.X - star.DownRight.X),
                Math.Abs(star.TopLeft.Y - star.DownRight.Y)) / 2;
            double innerRadius = outerRadius / 2;
 
            int points = 5;
            double angleIncrement = Math.PI / points;
            double startAngle = -Math.PI / 2;
 
            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                IsClosed = true,
                StartPoint = new Point(centerX + outerRadius * Math.Cos(startAngle),
                    centerY + outerRadius * Math.Sin(startAngle))
            };
 
            for (int i = 0; i < 2 * points; i++)
            {
                double radius = (i % 2 == 0) ? outerRadius : innerRadius;
                double angle = startAngle + i * angleIncrement;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);
                pathFigure.Segments.Add(new LineSegment(new Point(x, y), true));
            }
 
            pathGeometry.Figures.Add(pathFigure);
 
            Path path = new Path
            {
                Stroke = star.PenColor,
                Fill = star.BackgroundColor,
                Data = pathGeometry,
                StrokeThickness = star.StrokeThickness,
                RenderTransform = new RotateTransform(star.Angle, centerX, centerY),
            };
 
            return path;
        }
 
        return null;
    }
}