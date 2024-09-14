using System.Windows.Shapes;
using SharedComponents.AbstractClasses;

namespace SharedComponents.Interfaces;

public interface IDrawStrategy
{
    Shape Draw(AbstractShape shape);
}