namespace AKG.Core.Parser;

/// <summary>
/// Класс, описывающий грань (полигон). Грань может состоять из 3 и более вершин.
/// </summary>
public class Face
{
    public List<FaceVertex> Vertices { get; } = [];

    public override string ToString() => string.Join(" | ", Vertices);
}