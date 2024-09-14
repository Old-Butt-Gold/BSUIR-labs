using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures.Ellipse;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class EllipseFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyEllipse(topLeft, downRight, bgColor, penColor, angle);
    }
}