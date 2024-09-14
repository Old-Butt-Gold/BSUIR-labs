using System.Windows.Media;
using System.Windows.Shapes;
using OOTPiSP.DynamicLoad.GeometryFigures;
using SharedComponents;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace OOTPiSP.DynamicLoad.Strategy;

public class ArcDrawStrategy : IDrawStrategy
{
    public Shape Draw(AbstractShape shape)
    {
        if (shape is MyArc myArc)
        {
            double radiusX = Math.Abs(myArc.TopLeft.X - myArc.DownRight.X) / 2;
            double radiusY = Math.Abs(myArc.TopLeft.Y - myArc.DownRight.Y) / 2;
            double centerX = (myArc.TopLeft.X + myArc.DownRight.X) / 2;
            double centerY = (myArc.TopLeft.Y + myArc.DownRight.Y) / 2;

            double startAngle = Math.Atan2(myArc.TopLeft.Y - centerY, myArc.TopLeft.X - centerX) * 180 / Math.PI;
            double endAngle = Math.Atan2(myArc.DownRight.Y - centerY, myArc.DownRight.X - centerX) * 180 / Math.PI;

            PathGeometry pathGeometry = new PathGeometry();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new System.Windows.Point(centerX + radiusX * Math.Cos(startAngle * Math.PI / 180),
                    centerY + radiusY * Math.Sin(startAngle * Math.PI / 180))
            };

            ArcSegment arcSegment = new ArcSegment
            {
                Point = new System.Windows.Point(centerX + radiusX * Math.Cos(endAngle * Math.PI / 180),
                    centerY + radiusY * Math.Sin(endAngle * Math.PI / 180)),
                Size = new System.Windows.Size(radiusX, radiusY),
                IsLargeArc = Math.Abs(startAngle - endAngle) > 180,
                SweepDirection = SweepDirection.Clockwise
            };
            pathFigure.Segments.Add(arcSegment);
            pathGeometry.Figures.Add(pathFigure);
            
            Path path = new Path
            {
                Stroke = myArc.PenColor,
                Fill = myArc.BackgroundColor,
                Data = pathGeometry,
                RenderTransform = new RotateTransform(myArc.Angle, centerX, centerY),
                StrokeThickness = myArc.StrokeThickness,
            };

            return path;
        }

        return null;
    }
}