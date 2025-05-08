using System.Numerics;
using System.Windows.Media.Imaging;
using AKG.Core.Extensions;
using AKG.Core.Objects;

namespace AKG.Core.Renderer;

public class LightRenderer
{
    public static void DrawLights(Scene scene, WriteableBitmap wb)
    {
        var view = scene.Camera.GetViewMatrix();
        var projection = scene.Camera.GetProjectionMatrix();
        var viewport = scene.GetViewportMatrix();

        unsafe
        {
            var buffer = (int*)wb.BackBuffer;

            var forward = Vector3.Normalize(scene.Camera.Target - scene.Camera.Eye);
            foreach (var light in scene.Lights)
            {
                var lightVector = light.Position - scene.Camera.Eye;
                if (Vector3.Dot(lightVector, forward) <= 0)
                    continue; // Свет позади камеры – не рисуем

                var screenPosition = light.TransformLightToScreen(view, projection, viewport);
                var lightColor = (light.Color * 255f).ToColor().ColorToIntBgra();
                DrawLight(buffer, wb.PixelWidth, wb.PixelHeight, screenPosition, light.Intensity, lightColor);
            }
        }
    }

    public static unsafe void DrawLight(int* buffer, int width, int height, Vector3 screenPosition, float intensity,
        int color)
    {
        var x = (int)screenPosition.X;
        var y = (int)screenPosition.Y;

        var lightColor = color;

        var rayLength = 5;

        float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };

        foreach (var angle in angles)
        {
            var radians = angle * MathF.PI / 180.0f;

            var xEnd = x + (int)(rayLength * MathF.Cos(radians));
            var yEnd = y + (int)(rayLength * MathF.Sin(radians));

            WireframeRenderer.DrawLineBresenham(buffer, width, height, x, y, xEnd, yEnd, lightColor, 1);
        }

        WireframeRenderer.DrawLineBresenham(buffer, width, height, x, y, x, y, lightColor, 4);
        // Отрисовываем центральную точку (опционально)
        WireframeRenderer.DrawLineBresenham(buffer, width, height, x, y, x, y, lightColor, 3);

        // Отрисовываем круг вокруг центральной точки
        var radius = 10; // Радиус круга зависит от интенсивности
        DrawCircleBresenham(buffer, width, height, x, y, radius, lightColor);
    }

    public static unsafe void DrawCircleBresenham(int* buffer, int width, int height, int xc, int yc, int radius,
        int color)
    {
        var x = 0;
        var y = radius;
        var d = 3 - 2 * radius;

        while (x <= y)
        {
            DrawCirclePoints(buffer, width, height, xc, yc, x, y, color);

            if (d < 0)
            {
                d = d + 4 * x + 6;
            }
            else
            {
                d = d + 4 * (x - y) + 10;
                y--;
            }

            x++;
        }
    }

    private static unsafe void DrawCirclePoints(int* buffer, int width, int height, int xc, int yc, int x, int y, int color)
    {
        // Отрисовываем 8 симметричных точек окружности
        DrawPixel(buffer, width, height, xc + x, yc + y, color);
        DrawPixel(buffer, width, height, xc - x, yc + y, color);
        DrawPixel(buffer, width, height, xc + x, yc - y, color);
        DrawPixel(buffer, width, height, xc - x, yc - y, color);
        DrawPixel(buffer, width, height, xc + y, yc + x, color);
        DrawPixel(buffer, width, height, xc - y, yc + x, color);
        DrawPixel(buffer, width, height, xc + y, yc - x, color);
        DrawPixel(buffer, width, height, xc - y, yc - x, color);
        
        static void DrawPixel(int* buffer, int width, int height, int x, int y, int color)
        {
            // Проверяем, чтобы координаты были в пределах экрана
            if (x >= 0 && x < width && y >= 0 && y < height) buffer[y * width + x] = color;
        }
    }
}