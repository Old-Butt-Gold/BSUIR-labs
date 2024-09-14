using System.Windows.Media;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace AdapterShape.StarAdapter;

public class AdapterFactory : AbstractFactory
{
    public override AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle) 
        => new AdapterShape(topLeft, downRight, bgColor, penColor, angle);
}