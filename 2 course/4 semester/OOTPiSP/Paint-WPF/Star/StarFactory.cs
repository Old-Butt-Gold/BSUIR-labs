using System.Windows.Media;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace Star;

public class StarFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        return new StarShape(topLeft, downRight, bgColor, penColor, angle);
    }
}