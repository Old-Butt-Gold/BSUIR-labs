using System.Windows.Media;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace Star;

public class StarShape : AbstractShape
{
    public override object TagShape => "Star";

    public StarShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle) : base(topLeft, downRight, bgColor, penColor, angle)
    {
        DrawStrategy = new StarDrawStrategy();
    }

    public override string ToString() => "Звезда";
}