using System.Windows.Controls;
using System.Windows.Shapes;

namespace SharedComponents.AdapterShape;

public interface AbstractDrawStrategy
{
    Shape Draw(MySprite sprite, Canvas canvas);
}