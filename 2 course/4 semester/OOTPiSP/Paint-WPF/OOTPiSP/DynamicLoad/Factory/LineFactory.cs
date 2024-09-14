using System.Windows.Media;
using OOTPiSP.DynamicLoad.GeometryFigures;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP.DynamicLoad.Factory;

public class LineFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new MyLine(topLeft, downRight, bgColor, penColor, angle);
    }
}