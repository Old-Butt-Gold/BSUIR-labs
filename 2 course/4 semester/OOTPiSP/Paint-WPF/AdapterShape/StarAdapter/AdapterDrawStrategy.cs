using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SharedComponents.AbstractClasses;
using SharedComponents.AdapterShape;
using SharedComponents.Interfaces;

namespace AdapterShape.StarAdapter;

public class AdapterDrawStrategy : IDrawStrategy
{
    readonly MySprite _mySprite;

    public AdapterDrawStrategy(MySprite mySprite) => _mySprite = mySprite;

    public Shape Draw(AbstractShape shape)
    {
        _mySprite._rotationAngle = shape.Angle;
        _mySprite.FillColor = shape.BackgroundColor;
        _mySprite.StrokeColor = shape.PenColor;
        _mySprite.Points[0] = new Point(shape.TopLeft.X, shape.TopLeft.Y);
        _mySprite.Points[1] = new Point(shape.DownRight.X, shape.DownRight.Y);
        _mySprite.CalculateCenter();
        
        double width = Math.Abs(_mySprite.Points[0].X - _mySprite.Points[1].X);
        double height = Math.Abs(_mySprite.Points[0].Y - _mySprite.Points[1].Y);

        double centerX = _mySprite.Points[0].X + width / 2;
        double centerY = _mySprite.Points[0].Y + height / 2;

        double outerRadius = Math.Min(width, height) / 2;
        double innerRadius = outerRadius * 0.382; // Golden ratio 

        Point[] outerPoints = new Point[10];
        Point[] innerPoints = new Point[10];

        for (int i = 0; i < 10; i++)
        {
            double angleOuter = i * Math.PI / 5;
            double angleInner = (i + 0.5) * Math.PI / 5;

            outerPoints[i] = new Point(centerX + outerRadius * Math.Cos(angleOuter),
                centerY + outerRadius * Math.Sin(angleOuter));
            innerPoints[i] = new Point(centerX + innerRadius * Math.Cos(angleInner),
                centerY + innerRadius * Math.Sin(angleInner));
        }

        PathGeometry mySpriteGeometry = new PathGeometry();
        PathFigure mySpriteFigure = new PathFigure { StartPoint = outerPoints[0] };
        for (int i = 0; i < 10; i++)
        {
            int outerIndex = i % 10;
            int innerIndex = (i + 5) % 10;

            mySpriteFigure.Segments.Add(new LineSegment(outerPoints[outerIndex], true));
            mySpriteFigure.Segments.Add(new LineSegment(innerPoints[innerIndex], true));
        }

        mySpriteFigure.IsClosed = true;
        mySpriteGeometry.Figures.Add(mySpriteFigure);

        var path = new Path
        {
            Stroke = _mySprite.StrokeColor,
            Fill = _mySprite.FillColor,
            Data = mySpriteGeometry,
            StrokeThickness = _mySprite.StrokeThickness,
            RenderTransform = new RotateTransform(_mySprite._rotationAngle, centerX, centerY),
        };

        return path;
    }
}