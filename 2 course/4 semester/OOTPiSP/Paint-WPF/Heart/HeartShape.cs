using System.Windows.Media;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace Heart;

public class HeartShape : AbstractShape
{
    public override object TagShape => "Heart";

    public HeartShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle) : base(topLeft,
        downRight, bgColor, penColor, angle)
    {
        DrawStrategy = new HeartDrawStrategy();
    }

    public override string ToString() => "Сердце";
}