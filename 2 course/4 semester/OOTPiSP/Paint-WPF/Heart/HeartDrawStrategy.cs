using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace Heart;

public class HeartDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is HeartShape heart)
        {
            double width = Math.Abs(heart.TopLeft.X - heart.DownRight.X);
            double height = Math.Abs(heart.TopLeft.Y - heart.DownRight.Y);

            double centerX = heart.TopLeft.X + width / 2;
            double centerY = heart.TopLeft.Y + height / 2;

            PathFigure leftHalf = new PathFigure
            {
                StartPoint = new Point(centerX, centerY + height * 0.2)
            };
            leftHalf.Segments.Add(new BezierSegment(new Point(centerX - width * 0.4, centerY - height * 0.6),
                new Point(centerX - width * 0.8, centerY + height * 0.1),
                new Point(centerX, centerY + height * 0.9),
                true));

            PathFigure rightHalf = new PathFigure
            {
                StartPoint = new Point(centerX, centerY + height * 0.2)
            };
            rightHalf.Segments.Add(new BezierSegment(new Point(centerX + width * 0.4, centerY - height * 0.6),
                new Point(centerX + width * 0.8, centerY + height * 0.1),
                new Point(centerX, centerY + height * 0.9),
                true));

            PathGeometry heartGeometry = new PathGeometry();
            heartGeometry.Figures.Add(leftHalf);
            heartGeometry.Figures.Add(rightHalf);

            var path = new Path
            {
                Stroke = heart.PenColor,
                Fill = heart.BackgroundColor,
                Data = heartGeometry,
                StrokeThickness = heart.StrokeThickness,
                RenderTransform = new RotateTransform(heart.Angle, centerX, centerY),
            };
            
            return path;
        }

        return null;
    }
}