using System.Numerics;

namespace AKG.Core.Shadows;

/// <summary>
/// Структура, представляющая луч в пространстве.
/// Содержит начальную точку (Origin) и направление (Direction).
/// </summary>
public struct Ray 
{
    /// <summary>
    /// Начало луча в мировых координатах.
    /// </summary>
    public Vector3 Origin;
        
    /// <summary>
    /// Направление луча (должно быть нормализовано).
    /// </summary>
    public Vector3 Direction;
}