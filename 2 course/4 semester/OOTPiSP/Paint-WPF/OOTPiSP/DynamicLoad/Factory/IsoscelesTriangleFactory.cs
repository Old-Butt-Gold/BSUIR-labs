using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures.Triangle;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class IsoscelesTriangleFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyIsoscelesTriangle(topLeft, downRight, bgColor, penColor, angle);
    }
}