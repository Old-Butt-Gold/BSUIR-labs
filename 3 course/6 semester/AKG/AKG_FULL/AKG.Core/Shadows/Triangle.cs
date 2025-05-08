using System.Numerics;

namespace AKG.Core.Shadows;


/// <summary>
/// Представляет треугольник для расчёта теней.
/// </summary>
public class Triangle
{
    public const float Bias = 0.0001f;
    
    public Vector3 V0, V1, V2;
    public Vector3 Center => (V0 + V1 + V2) / 3.0f;

    /// <summary>
    /// Проверяет пересечение луча с треугольником с использованием алгоритма Моллера-Трумбора.
    /// Если пересечение найдено, возвращает true и расстояние до пересечения в параметре t.
    /// </summary>
    /// <param name="ray">Луч, для которого проверяется пересечение</param>
    /// <param name="t">Расстояние от начала луча до точки пересечения</param>
    /// <returns>True, если пересечение найдено, иначе false</returns>
    public bool Intersect(Ray ray, out float t)
    {
        t = 0f;
        // Вычисляем два ребра треугольника
        var edge1 = V1 - V0;
        var edge2 = V2 - V0;
        
        // 1. Вычисляем вектор, перпендикулярный направлению луча и второму ребру
        var h = Vector3.Cross(ray.Direction, edge2);
        // 2. Вычисляем скалярное произведение первого ребра и вектора h
        var a = Vector3.Dot(edge1, h);
        
        // Если a близко к нулю, луч параллелен плоскости треугольника
        if (MathF.Abs(a) < float.Epsilon)
            return false;

        var f = 1.0f / a;
        var s = ray.Origin - V0;
        
        // 3. Вычисляем первую барицентрическую координату (u)
        var u = f * Vector3.Dot(s, h);
        if (u is < 0 or > 1)
            return false;

        // 4. Вычисляем второй барицентрическую параметр (v)
        var q = Vector3.Cross(s, edge1);
        var v = f * Vector3.Dot(ray.Direction, q);
        if (v < 0 || u + v > 1)
            return false;

        // 5. Вычисление расстояния до точки пересечения
        t = f * Vector3.Dot(edge2, q);
        
        // Проверяем, находится ли точка пересечения перед началом луча с учётом bias
        return t > Bias; 
    }
}