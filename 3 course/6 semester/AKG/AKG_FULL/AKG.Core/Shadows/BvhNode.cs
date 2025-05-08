namespace AKG.Core.Shadows;

/// <summary>
/// Узел иерархии ограничивающих объёмов (Bounding Volume Hierarchy).
/// Каждый узел содержит AABB, а также либо дочерние узлы (для внутренних узлов),
/// либо список треугольников (для листовых узлов).
/// </summary>
public class BvhNode
{
    /// <summary>
    /// Ограничивающий объём (AABB) узла.
    /// Он должен охватывать все треугольники или дочерние узлы, входящие в данный узел.
    /// </summary>
    public AABB Bounds; 
    public BvhNode? Left; // Левый дочерний узел
    public BvhNode? Right; // Правый дочерний узел
    public List<Triangle>? Triangles; // Треугольники (только в листьях)
    public bool IsLeaf => Triangles != null;
}