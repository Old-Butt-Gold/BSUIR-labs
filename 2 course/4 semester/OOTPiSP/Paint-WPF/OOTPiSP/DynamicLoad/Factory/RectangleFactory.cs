using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures.Rectangle;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class RectangleFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyRectangle(topLeft, downRight, bgColor, penColor, angle);
    }
}