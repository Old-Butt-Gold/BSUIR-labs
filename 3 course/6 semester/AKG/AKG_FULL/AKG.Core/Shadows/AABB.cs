using System.Numerics;

namespace AKG.Core.Shadows;

/// <summary>
/// Axis-Aligned Bounding Box (AABB) используется для быстрого отсечения при проверке пересечений лучей.
/// AABB определяется минимальной и максимальной точками, охватывающими объект.
/// </summary>
public struct AABB
{
    /// <summary>
    /// Минимальная точка ограничивающего объёма по каждой оси.
    /// </summary>
    public Vector3 Min { get; }
    /// <summary>
    /// Максимальная точка ограничивающего объёма по каждой оси.
    /// </summary>
    public Vector3 Max { get; }
    
    public AABB(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
    }
    
    /// <summary>
    /// Проверяет пересечение луча с AABB, используя алгоритм Kay-Kajiya.
    /// Для каждой оси рассчитываются интервалы t, на которых луч пересекает плоскости AABB.
    /// Если найдено пересечение по всем осям, возвращается true.
    /// </summary>
    /// <param name="ray">Луч, пересечение которого проверяется</param>
    /// <returns>True, если луч пересекает AABB, иначе false</returns>
    public bool IntersectRay(Ray ray)
    {
        float tmin = 0.0f;
        float tmax = float.MaxValue;

        // Проходим по каждой из трех осей (X, Y, Z)
        for (int axis = 0; axis < 3; axis++)
        {
            // Вычисляем обратное значение направления для текущей оси
            float invDir = 1.0f / ray.Direction[axis];
            // Вычисляем параметры пересечения луча с плоскостями AABB по текущей оси
            float t0 = (Min[axis] - ray.Origin[axis]) * invDir;
            float t1 = (Max[axis] - ray.Origin[axis]) * invDir;

            // Если направление луча отрицательное, меняем местами t0 и t1
            if (invDir < 0.0f)
                (t0, t1) = (t1, t0);

            // Обновляем интервал пересечения
            tmin = MathF.Max(t0, tmin);
            tmax = MathF.Min(t1, tmax);

            // Если интервал не существует, луч не пересекает AABB
            if (tmax <= tmin)
                return false;
        }

        return true;
    }
}