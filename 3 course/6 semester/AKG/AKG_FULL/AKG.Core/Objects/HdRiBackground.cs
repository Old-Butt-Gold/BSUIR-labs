using System.IO;
using System.Numerics;
using StbImageSharp;

namespace AKG.Core.Objects;

public class HdRiBackground
{
    private int _height;
    private Vector3[] _pixels = [];
    private int _width;

    public void LoadFromHdrFile(string path)
    {
        using var stream = File.OpenRead(path);
        var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlue);

        _width = image.Width;
        _height = image.Height;
        _pixels = new Vector3[_width * _height];

        // Используем данные в формате float для HDR
        var floatData = image.Data;

        Parallel.For(0, _pixels.Length, i =>
        {
            var offset = i * 3;
            _pixels[i] = new Vector3(
                floatData[offset],
                floatData[offset + 1],
                floatData[offset + 2]
            );
        });
    }

    public Vector3 SampleSpherical(Vector3 direction)
    {
        var u = (MathF.Atan2(direction.Z, direction.X) + MathF.PI) / (2 * MathF.PI);
        var v = MathF.Acos(direction.Y) / MathF.PI;

        return SampleUv(u, v);
    }

    private Vector3 SampleUv(float u, float v)
    {
        var x = (int)(u * _width) % _width;
        var y = (int)(v * _height) % _height;
        return _pixels[y * _width + x];
    }
}