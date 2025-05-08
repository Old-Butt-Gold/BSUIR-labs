using System.Numerics;
using AKG.Core.VectorTransformations;

namespace AKG.Core.Objects;

public class Camera
{
    /// <summary>
    ///     Позиция камеры в мировом пространстве
    /// </summary>
    public Vector3 Eye { get; set; } = Vector3.Zero;

    /// <summary>
    ///     Позиция цели, на которую направлена камера
    /// </summary>
    public Vector3 Target { get; set; } = Vector3.Zero;

    /// <summary>
    ///     Вектор, направленный вертикально вверх с точки зрения камеры (Ось Y)
    /// </summary>
    public Vector3 Up { get; set; } = Vector3.UnitY;

    /// <summary>
    ///     Поле зрения камеры по оси Y (в радианах) (90 градусов)
    /// </summary>
    public float Fov { get; set; } = MathF.PI / 2.0f;

    /// <summary>
    ///     Соотношение сторон обзора камеры
    /// </summary>
    public float Aspect { get; set; } = 16f / 9f;

    /// <summary>
    ///     Расстояние до ближней плоскости обзора. Используется для отсечения объектов, находящихся слишком близко к камере.
    /// </summary>
    public float ZNear { get; set; } = 0.01f;

    /// <summary>
    ///     Расстояние до дальней плоскости обзора. Используется для отсечения объектов, находящихся слишком далеко от камеры.
    /// </summary>
    public float ZFar { get; set; } = 100f;

    /// <summary>
    ///     Радиус орбиты камеры вокруг цели. Определяет расстояние между позицией камеры и её целью.
    /// </summary>
    public float Radius { get; set; } = 5;

    /// <summary>
    ///     Угол ζ (Zeta) в сферических координатах, определяющий вертикальное положение камеры относительно цели.
    ///     Диапазон: [0, π].
    /// </summary>
    public float Zeta { get; set; } = (float)Math.PI / (float)2.3;

    /// <summary>
    ///     Угол φ (Phi) в сферических координатах, определяющий горизонтальное положение камеры относительно цели.
    ///     Диапазон: [0, 2π].
    /// </summary>
    public float Phi { get; set; } = (float)Math.PI / 2;

    public Matrix4x4 GetViewMatrix()
    {
        return Transformations.CreateViewMatrix(Eye, Target, Up);
    }

    public Matrix4x4 GetProjectionMatrix()
    {
        return Transformations.CreatePerspectiveProjection(Fov, Aspect, ZNear, ZFar);
    }

    /// <summary>
    ///     Обновляет позицию камеры (Eye) на основе сферических координат (Radius, Zeta, Phi).
    /// </summary>
    public void ChangeEye()
    {
        // Вычисляем новую позицию камеры с использованием сферических координат:
        // X = Radius * cos(Phi) * sin(Zeta)
        // Y = Radius * cos(Zeta)
        // Z = Radius * sin(Phi) * sin(Zeta)
        Eye = new Vector3(
            Radius * MathF.Cos(Phi) * MathF.Sin(Zeta),
            Radius * MathF.Cos(Zeta),
            Radius * MathF.Sin(Phi) * MathF.Sin(Zeta));
    }
}