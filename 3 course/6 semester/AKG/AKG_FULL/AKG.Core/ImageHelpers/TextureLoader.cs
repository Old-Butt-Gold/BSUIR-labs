using System.Collections.Concurrent;
using System.IO;
using System.Windows.Media.Imaging;

namespace AKG.Core.ImageHelpers;

public static class TextureLoader
{
    private static readonly ConcurrentDictionary<string, BitmapImage> Cache = new();

    public static BitmapImage? Load(string path)
    {
        if (Cache.TryGetValue(path, out var image))
            return image;

        if (!File.Exists(path)) return null;

        var bitmap = new BitmapImage();
        bitmap.BeginInit();
        bitmap.UriSource = new Uri(path);
        bitmap.CacheOption = BitmapCacheOption.OnLoad;
        bitmap.EndInit();
        bitmap.Freeze();

        Cache[path] = bitmap;
        return bitmap;
    }

    public static void ClearCache()
    {
        Cache.Clear();
    }
}