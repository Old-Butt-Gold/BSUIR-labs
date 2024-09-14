using System.Windows.Media;

namespace SharedComponents.AbstractClasses;

public abstract class AbstractFactory
{
    public abstract AbstractShape CreateShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle);
}