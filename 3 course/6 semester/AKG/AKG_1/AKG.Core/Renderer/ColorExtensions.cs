using System.Windows.Media;

namespace AKG.Core.Renderer;

public static class ColorExtensions
{
    public static int ColorToIntBGRA(this Color color)
    {
        return (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
    }
}