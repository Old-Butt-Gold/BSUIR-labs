namespace AKG.Core.Objects;

/// <summary>
///     Класс, описывающий грань (полигон). Грань может состоять из 3 и более вершин.
/// </summary>
public class Face
{
    public List<FaceVertex> Vertices { get; } = [];

    // Имя материала, применяемого к грани (записывается из usemtl)
    public string MaterialName { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Material – {MaterialName} : " + string.Join(" | ", Vertices);
    }
}