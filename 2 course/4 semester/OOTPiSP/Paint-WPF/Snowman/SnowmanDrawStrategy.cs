using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace Snowman;

public class SnowmanDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is SnowmanShape snowman)
        {
            double centerX = (snowman.TopLeft.X + snowman.DownRight.X) / 2;
            double centerY = (snowman.TopLeft.Y + snowman.DownRight.Y) / 2;

            PathGeometry headGeometry = new PathGeometry();
            EllipseGeometry headEllipse = new EllipseGeometry(new Point(centerX, centerY - 60), 30, 30);
            headGeometry.AddGeometry(headEllipse);

            PathGeometry bodyGeometry = new PathGeometry();
            EllipseGeometry bodyEllipse = new EllipseGeometry(new Point(centerX, centerY + 20), 50, 50);
            bodyGeometry.AddGeometry(bodyEllipse);

            EllipseGeometry leftEye = new EllipseGeometry(new Point(centerX - 10, centerY - 65), 5, 5);
            EllipseGeometry rightEye = new EllipseGeometry(new Point(centerX + 10, centerY - 65), 5, 5);

            PathGeometry mouthGeometry = new PathGeometry();
            PathFigure mouthFigure = new PathFigure
            {
                StartPoint = new Point(centerX - 15, centerY - 55)
            };
            BezierSegment bezierSegment = new BezierSegment(new Point(centerX - 10, centerY - 50),
                new Point(centerX + 10, centerY - 50),
                new Point(centerX + 15, centerY - 55),
                true);
            mouthFigure.Segments.Add(bezierSegment);
            mouthGeometry.Figures.Add(mouthFigure);
            mouthFigure.IsClosed = true;

            PathGeometry hatGeometry = new PathGeometry();
            PathFigure hatFigure = new PathFigure
            {
                StartPoint = new Point(centerX - 25, centerY - 90)
            };
            LineSegment hatLine1 = new LineSegment(new Point(centerX + 25, centerY - 90), true);
            LineSegment hatLine2 = new LineSegment(new Point(centerX + 15, centerY - 120), true);
            LineSegment hatLine3 = new LineSegment(new Point(centerX - 15, centerY - 120), true);
            hatFigure.Segments.Add(hatLine1);
            hatFigure.Segments.Add(hatLine2);
            hatFigure.Segments.Add(hatLine3);
            hatFigure.IsClosed = true;
            hatGeometry.Figures.Add(hatFigure);

            GeometryGroup snowmanGeometry = new GeometryGroup();
            snowmanGeometry.Children.Add(headGeometry);
            snowmanGeometry.Children.Add(bodyGeometry);
            snowmanGeometry.Children.Add(leftEye);
            snowmanGeometry.Children.Add(rightEye);
            snowmanGeometry.Children.Add(mouthGeometry);
            snowmanGeometry.Children.Add(hatGeometry);

            Path path = new Path
            {
                Stroke = snowman.PenColor,
                Fill = snowman.BackgroundColor,
                Data = snowmanGeometry,
                StrokeThickness = snowman.StrokeThickness,
                RenderTransform = new RotateTransform(snowman.Angle, centerX, centerY),
            };

            return path;
        }

        return null;
    }
}