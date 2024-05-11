using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace MIAPR_9;

public class BitmapConverter
{
    public static List<int> ToInt32List(Bitmap bitmap)
    {
        List<int> result = [];
        for (var i = 0; i < bitmap.Width; i++)
        {
            for (var j = 0; j < bitmap.Height; j++)
            {
                var pixelColor = bitmap.GetPixel(i, j);
                result.Add(255 - (pixelColor.R + pixelColor.G + pixelColor.B) / 3);
            }
        }
        return result;
    }

    public static Bitmap Load(string path, int size) => new(new Bitmap(path), size, size);

    public static BitmapImage ToBitmapImage(Bitmap bitmap)
    {
        var bitmapSource = new BitmapImage();
        
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        
        bitmapSource.BeginInit();
        bitmapSource.StreamSource = new MemoryStream(stream.ToArray());
        bitmapSource.EndInit();
        return bitmapSource;
    }
}