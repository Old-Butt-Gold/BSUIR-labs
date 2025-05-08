using System.Collections.Concurrent;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AKG.Core.ImageHelpers;

public static class TextureSampler
{
    // Кэш для хранения массивов пикселей по каждой текстуре.
    private static readonly ConcurrentDictionary<BitmapImage, byte[]> TextureCache = [];

    /// <summary>
    ///     Выбирает (sample) цвет из текстуры по заданным координатам u и v.
    ///     Предполагается, что u,v ∈ [0,1]. Координата v инвертируется, поскольку
    ///     изображения WPF имеют начало координат в верхнем левом углу.
    /// </summary>
    public static Color Sample(BitmapImage texture, float u, float v)
    {
        var width = texture.PixelWidth;
        var height = texture.PixelHeight;

        // Приводим u,v к пиксельным координатам.
        var x = (int)(u * width);
        var y = (int)((1.0f - v) * height);

        x = Math.Clamp(x, 0, width - 1);
        y = Math.Clamp(y, 0, height - 1);

        var pixels = GetPixels(texture);
        return GetColorAt(pixels, width, x, y);
    }

    private static byte[] GetPixels(BitmapImage texture)
    {
        // Используем кэш для ускорения
        if (!TextureCache.TryGetValue(texture, out var pixels))
        {
            var width = texture.PixelWidth;
            var height = texture.PixelHeight;
            var stride = width * 4;
            pixels ??= new byte[height * stride];
            texture.CopyPixels(pixels, stride, 0);
            TextureCache[texture] = pixels;
        }

        return pixels;
    }

    private static Color GetColorAt(byte[] pixels, int width, int x, int y)
    {
        var index = (y * width + x) * 4;
        var b = pixels[index];
        var g = pixels[index + 1];
        var r = pixels[index + 2];
        var a = pixels[index + 3];
        return Color.FromArgb(a, r, g, b);
    }

    public static void ClearCache()
    {
        TextureCache.Clear();
    }
}