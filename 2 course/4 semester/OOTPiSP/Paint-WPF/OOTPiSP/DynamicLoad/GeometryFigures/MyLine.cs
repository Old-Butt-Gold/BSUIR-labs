using System.Windows.Media;
using OOTPiSP.DynamicLoad.Strategy;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.GeometryFigures;

public class MyLine : AbstractShape
{
    public override object TagShape => "4";

    public MyLine(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush borderColor, int angle)
        : base(topLeft, downRight, bgColor, borderColor, angle)
    {
        DrawStrategy = new LineDrawStrategy();
    }

    public override string ToString() =>
        $"Линия";

}