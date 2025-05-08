using System.Numerics;
using System.Windows.Media;

namespace AKG.Core.Extensions;

public static class ColorExtensions
{
    public static int ColorToIntBgra(this Color color)
    {
        return (color.B << 0) | (color.G << 8) | (color.R << 16) | (color.A << 24);
    }

    public static Vector3 ToVector3(this Color color)
    {
        return new Vector3(color.R, color.G, color.B);
    }

    public static Color ToColor(this Vector3 vector)
    {
        return Color.FromArgb(255, (byte)vector.X, (byte)vector.Y, (byte)vector.Z);
    }
}