using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures.Triangle;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class EquilateralTriangleFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new EquilateralMyTriangle(topLeft, downRight, bgColor, penColor, angle);
    }
}