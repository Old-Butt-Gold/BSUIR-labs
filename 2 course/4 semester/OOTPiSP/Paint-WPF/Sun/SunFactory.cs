using System.Windows.Media;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace Sun;

public class SunFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle) 
        => new SunShape(topLeft, downRight, bgColor, penColor, angle);
}