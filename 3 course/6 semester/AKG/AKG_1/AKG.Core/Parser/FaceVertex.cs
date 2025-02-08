namespace AKG.Core.Parser;

/// <summary>
/// Структура, описывающая один элемент грани (индексы вершины, текстурной координаты и нормали).
/// Если индекс отсутствует в файле, то он будет равен 0.
/// </summary>
public struct FaceVertex
{
    // Сохраняем индексы как записаны в файле (начинаются с 1, отрицательные – обратный отсчёт)
    public int VertexIndex;      // Индекс вершины (v), всегда присутствует
    public int TextureIndex;     // Индекс текстурной координаты (vt), может отсутствовать
    public int NormalIndex;      // Индекс нормали (vn), может отсутствовать

    public override string ToString() => $"v:{VertexIndex} vt:{TextureIndex} vn:{NormalIndex}";
}