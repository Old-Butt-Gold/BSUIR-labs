using System.Windows.Media;
using OOTPiSP.DynamicLoad.Strategy;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.GeometryFigures;

public class MyArc : AbstractShape
{
    public override object TagShape => "8";

    public MyArc(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
        : base(topLeft, downRight, bgColor, penColor, angle)
    {
        DrawStrategy = new ArcDrawStrategy();
    }

    public override string ToString() => $"Дуга";
}