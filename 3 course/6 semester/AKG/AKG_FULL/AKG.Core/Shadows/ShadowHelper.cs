namespace AKG.Core.Shadows;

/// <summary>
/// Класс для работы с тенями и трассировкой лучей.
/// Данный метод выполняет обход BVH-дерева, чтобы быстро определить,
/// пересекается ли заданный луч с каким-либо треугольником модели.
/// </summary>
public static class ShadowHelper
{
    /// <summary>
    /// Рекурсивно проверяет пересечение луча с BVH-деревом.
    /// Если найдено пересечение, возвращает расстояние до ближайшей точки пересечения (closestT).
    /// </summary>
    /// <param name="node">Корневой узел BVH-дерева</param>
    /// <param name="ray">Трассируемый луч</param>
    /// <param name="closestT">Расстояние до ближайшего пересечения</param>
    /// <returns>True, если луч пересекается с каким-либо объектом в дереве, иначе false</returns>
    public static bool RayIntersectBvh(BvhNode node, Ray ray, out float closestT)
    {
        // Изначально устанавливаем расстояние до пересечения как максимально возможное значение.
        closestT = float.MaxValue;

        // 1. Проверка пересечения луча с ограничивающим объёмом (AABB) текущего узла.
        // Если луч не проходит через AABB, дальнейший обход этого узла не имеет смысла.
        if (!node.Bounds.IntersectRay(ray))
            return false;

        // 2. Если текущий узел является листом (то есть содержит список треугольников),
        // проверяем пересечения луча со всеми треугольниками в узле.
        if (node.IsLeaf)
        {
            bool hit = false;
            foreach (var tri in node.Triangles!)
            {
                // Метод Intersect проверяет пересечение луча с треугольником по алгоритму Моллера-Трумбора.
                // Если пересечение найдено и расстояние до пересечения меньше текущего closestT,
                // обновляем значение и отмечаем, что произошло пересечение.
                if (tri.Intersect(ray, out var t) && t < closestT)
                {
                    closestT = t;
                    hit = true;
                }
            }

            return hit;
        }

        // 3. Если узел не является листом, рекурсивно проверяем пересечения в его дочерних узлах.
        // Сначала проверяем левый дочерний узел.
        bool leftHit = RayIntersectBvh(node.Left!, ray, out float leftT);
        // Затем проверяем правый дочерний узел.
        bool rightHit = RayIntersectBvh(node.Right!, ray, out float rightT);

        // Определяем ближайшее пересечение из найденных в дочерних узлах.
        closestT = Math.Min(leftT, rightT);

        // Если хотя бы в одном из дочерних узлов обнаружено пересечение, возвращаем true.
        return leftHit || rightHit;
    }
}