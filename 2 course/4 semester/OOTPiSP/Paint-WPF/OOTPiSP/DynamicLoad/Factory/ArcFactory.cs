using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class ArcFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyArc(topLeft, downRight, bgColor, penColor, angle);
    }
}