using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace AKG.UI.Converters;

/// <summary>
/// Converter для преобразования цвета, заданного в виде Vector3 с компонентами в диапазоне [0,1],
/// в строку с числовыми значениями (0-255) и обратно.
/// </summary>
public class ColorVectorConverter : IValueConverter
{
    // Преобразует нормализованный вектор (0-1) в строку с компонентами от 0 до 255
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Vector3 vec)
        {
            float r = vec.X * 255;
            float g = vec.Y * 255;
            float b = vec.Z * 255;
            return string.Format(CultureInfo.InvariantCulture, "{0:F0}; {1:F0}; {2:F0}", r, g, b);
        }
        return string.Empty;
    }

    // Преобразует строку вида "R; G; B" в нормализованный вектор (0-1)
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string s)
        {
            string[] parts = s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 &&
                float.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out float r) &&
                float.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out float g) &&
                float.TryParse(parts[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out float b))
            {
                var vector = Vector3.Clamp(new Vector3(r, g, b), Vector3.Zero, new(255));
                return new Vector3(vector.X / 255f, vector.Y / 255f, vector.Z / 255f);
            }
        }
        return new Vector3(0, 0, 0);
    }
}