using System.Numerics;

namespace AKG.Core.Shadows;

/// <summary>
/// Построитель BVH для ускорения трассировки лучей
/// </summary>
public static class BvhBuilder
{
    // Минимальное количество треугольников в листе
    private const int LeafThreshold = 4;

    public static BvhNode BuildBvh(List<Triangle> triangles)
    {
        if (triangles.Count == 0)
            throw new ArgumentException("Triangles list is empty");

        return BuildRecursive(triangles);
    }

    private static BvhNode BuildRecursive(List<Triangle> triangles)
    {
        var node = new BvhNode();

        if (triangles.Count <= LeafThreshold)
        {
            node.Triangles = triangles;
            node.Bounds = CalculateAABB(triangles); // Вычисляем AABB охватывающий все треугольники
            return node;
        }

        // Разбиваем треугольники на две группы по медиане центра вдоль наиболее протяжённой оси
        var (leftTriangles, rightTriangles) = SplitTriangles(triangles);

        // Рекурсивно строим дочерние узлы
        node.Left = BuildRecursive(leftTriangles);
        node.Right = BuildRecursive(rightTriangles);
        // Объединяем AABB дочерних узлов, чтобы получить AABB для текущего узла
        node.Bounds = Union(node.Left.Bounds, node.Right.Bounds);

        return node;
    }
    
    /// <summary>
    /// Вычисляет Axis-Aligned Bounding Box (AABB) для списка треугольников.
    /// Перебираются все треугольники, и для каждой вершины находится минимальное и максимальное значение по каждой оси.
    /// </summary>
    private static AABB CalculateAABB(List<Triangle> triangles)
    {
        var min = new Vector3(float.MaxValue);
        var max = new Vector3(float.MinValue);

        foreach (var tri in triangles)
        {
            // Вычисляем минимума и максимума по каждому из вершин треугольника
            min = Vector3.Min(min, Vector3.Min(tri.V0, Vector3.Min(tri.V1, tri.V2)));
            max = Vector3.Max(max, Vector3.Max(tri.V0, Vector3.Max(tri.V1, tri.V2)));
        }

        return new AABB(min, max);
    }
    
    /// <summary>
    /// Объединяет два AABB в один, который полностью охватывает оба входных ограничивающих объёма.
    /// </summary>
    private static AABB Union(AABB a, AABB b)
    {
        var min = Vector3.Min(a.Min, b.Min);
        var max = Vector3.Max(a.Max, b.Max);
        return new AABB(min, max);
    }
    
    /// <summary>
    /// Разбивает список треугольников на две части по медиане центра вдоль наиболее протяжённой оси.
    /// Определяется ось с максимальным размером ограничивающего объёма, затем треугольники сортируются по координате центра вдоль этой оси.
    /// </summary>
    private static (List<Triangle> left, List<Triangle> right) SplitTriangles(List<Triangle> triangles)
    {
        // Вычисляем общий AABB для списка треугольников
        var bounds = CalculateAABB(triangles);
        var extent = bounds.Max - bounds.Min;

        // Определяем ось с максимальным размером (0 - X, 1 - Y, 2 - Z)
        int axis = 0;
        if (extent.Y > extent.X && extent.Y >= extent.Z)
            axis = 1;
        else if (extent.Z > extent.X && extent.Z >= extent.Y)
            axis = 2;

        // Сортируем треугольники по координате центра вдоль выбранной оси
        triangles.Sort((t1, t2) 
            => t1.Center[axis].CompareTo(t2.Center[axis]));

        int mid = triangles.Count / 2;
        var left = triangles.Take(mid).ToList();
        var right = triangles.Skip(mid).ToList();

        // Если по какой-то причине одна из групп оказалась пустой, перераспределяем
        if (left.Count == 0 || right.Count == 0)
        {
            left = triangles.Take(triangles.Count / 2).ToList();
            right = triangles.Skip(triangles.Count / 2).ToList();
        }

        return (left, right);
    }
}