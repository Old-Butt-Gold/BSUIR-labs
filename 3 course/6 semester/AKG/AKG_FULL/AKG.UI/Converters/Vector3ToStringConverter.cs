using System.Globalization;
using System.Numerics;
using System.Windows.Data;

namespace AKG.UI.Converters;

public class Vector3ToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Vector3 vec)
            return string.Format(CultureInfo.InvariantCulture, "{0:F3}; {1:F3}; {2:F3}", vec.X, vec.Y, vec.Z);
        return string.Empty;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string str)
        {
            string[] parts = str.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 3 &&
                float.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var x) &&
                float.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var y) &&
                float.TryParse(parts[2].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var z))
                return new Vector3(x, y, z);
        }

        return new Vector3(0, 0, 0);
    }
}