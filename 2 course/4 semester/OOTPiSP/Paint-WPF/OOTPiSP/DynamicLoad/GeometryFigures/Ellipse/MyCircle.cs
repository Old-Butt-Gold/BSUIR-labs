using System.Windows.Media;
using SharedComponents;

namespace OOTPiSP.DynamicLoad.GeometryFigures.Ellipse;

public class MyCircle : MyEllipse
{
    public override object TagShape => "0";

    public MyCircle(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush borderColor, int angle)
        : base(topLeft, downRight, bgColor, borderColor, angle)
    { }
    
    public override double GetHeight() => Math.Abs(TopLeft.X - DownRight.X);
    public override double GetWidth() => Math.Abs(TopLeft.X - DownRight.X);

    public override string ToString() =>
        $"Круг";

}