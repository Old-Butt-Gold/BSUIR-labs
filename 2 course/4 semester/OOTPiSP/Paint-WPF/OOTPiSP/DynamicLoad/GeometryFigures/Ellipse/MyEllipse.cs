using System.Windows.Media;
using OOTPiSP.DynamicLoad.Strategy;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.GeometryFigures.Ellipse;

public class MyEllipse : AbstractShape
{
    public override object TagShape => "1";
    
    public MyEllipse(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
        : base(topLeft, downRight, bgColor, penColor, angle)
    {
        DrawStrategy = new EllipseDrawStrategy();
    }
    
    public virtual double GetWidth() => Math.Abs(TopLeft.X - DownRight.X);
    public virtual double GetHeight() => Math.Abs(TopLeft.Y - DownRight.Y);

    public override string ToString() =>
        $"Эллипс";

}