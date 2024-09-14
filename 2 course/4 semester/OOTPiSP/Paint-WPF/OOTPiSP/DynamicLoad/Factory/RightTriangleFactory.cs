using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures.Triangle;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class RightTriangleFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyRightTriangle(topLeft, downRight, bgColor, penColor, angle);
    }
}