using System.Windows;
using System.Windows.Media;
using SharedComponents.AdapterShape;

namespace AdapterShape.External_Plugins;

public class StarShape : MySprite
{
    public override object idOfClassShape { get; } = "Star";

    public StarShape( Brush fillColor, Brush strokeColor, Point[] points, double rotationAngle) : base(fillColor,
        strokeColor, points, rotationAngle)
    {
        //DrawStrategy = new StrategyForStar();
    }
    public override int countOfPoints => 2;
    public override string ToString() => $"Звезда";
}