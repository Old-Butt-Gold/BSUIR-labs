using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AKG.Core.Parser;

namespace AKG.Core.Renderer;

public static class WireframeRenderer
{
    /// <summary>
    /// Рисует проволочную 3D модель с использованием алгоритма Брезенхэма для растеризации линий.
    /// Рисование производится на WriteableBitmap, которая затем может быть установлена, например, как Source для Image.
    /// </summary>
    /// <param name="model">Объект модели с заполненным списком TransformedVertices</param>
    /// <param name="wb">WriteableBitmap, куда будут записаны пиксели</param>
    /// <param name="color">Цвет линий</param>
    public static void DrawWireframe(ObjModel model, WriteableBitmap wb, Color color)
    {
        // Определим цвет в формате BGRA (WriteableBitmap обычно использует PixelFormat Bgra32)
        int intColor = color.ColorToIntBGRA();

        wb.Lock();

        unsafe
        {
            // Получаем указатель на начало буфера пикселей
            int* pBackBuffer = (int*)wb.BackBuffer;
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;

            // Для каждой грани модели
            foreach (var face in model.Faces)
            {
                int count = face.Vertices.Count;
                if (count < 2)
                    continue;

                for (int i = 0; i < count; i++)
                {
                    // Индексы в файле OBJ начинаются с 1, поэтому вычитаем 1
                    int index1 = face.Vertices[i].VertexIndex - 1;
                    int index2 = face.Vertices[(i + 1) % count].VertexIndex - 1;

                    // Проверяем диапазон индексов
                    if (index1 < 0 || index1 >= model.TransformedVertices.Length ||
                        index2 < 0 || index2 >= model.TransformedVertices.Length)
                        continue;

                    // Получаем экранные координаты (используем double для вычислений, затем преобразуем к int)
                    int x0 = (int)Math.Round(model.TransformedVertices[index1].X);
                    int y0 = (int)Math.Round(model.TransformedVertices[index1].Y);
                    int x1 = (int)Math.Round(model.TransformedVertices[index2].X);
                    int y1 = (int)Math.Round(model.TransformedVertices[index2].Y);

                    // Рисуем линию алгоритмом Брезенхэма
                    DrawLineBresenham(pBackBuffer, width, height, x0, y0, x1, y1, intColor);
                }
            }
        }

        // Сообщаем системе, что изменился весь буфер
        wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
        wb.Unlock();
    }
    
    /// <summary>
    /// Рисует линию с помощью алгоритма Брезенхэма.
    /// Работает с указателем на BackBuffer WriteableBitmap.
    /// </summary>
    /// <param name="buffer">Указатель на массив пикселей</param>
    /// <param name="width">Ширина изображения (в пикселях)</param>
    /// <param name="height">Высота изображения (в пикселях)</param>
    /// <param name="x0">Начальная координата X</param>
    /// <param name="y0">Начальная координата Y</param>
    /// <param name="x1">Конечная координата X</param>
    /// <param name="y1">Конечная координата Y</param>
    /// <param name="color">Цвет линии в формате ARGB (целое число)</param>
    public static unsafe void DrawLineBresenham(int* buffer, int width, int height, int x0, int y0, int x1, int y1, int color)
    {
        int dx = Math.Abs(x1 - x0);
        int dy = Math.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            // Если координаты внутри экрана, установим пиксель
            if (x0 >= 0 && x0 < width && y0 >= 0 && y0 < height)
            {
                buffer[y0 * width + x0] = color;
            }

            if (x0 == x1 && y0 == y1)
                break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }
    
    public static void ClearBitmap(WriteableBitmap wb, Color clearColor)
    {
        int intColor = (clearColor.A << 24) | (clearColor.R << 16) | (clearColor.G << 8) | clearColor.B;

        wb.Lock();

        try
        {
            unsafe
            {
                int* pBackBuffer = (int*)wb.BackBuffer;

                for (int i = 0; i < wb.PixelHeight; i++)
                {
                    for (int j = 0; j < wb.PixelWidth; j++)
                    {
                        *pBackBuffer++ = intColor;
                    }
                }
            }

            wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
        }
        finally
        {
            wb.Unlock();
        }
    }
}