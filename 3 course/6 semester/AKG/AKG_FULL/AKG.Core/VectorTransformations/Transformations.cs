using System.Numerics;

namespace AKG.Core.VectorTransformations;

public static class Transformations
{
    /// <summary>
    ///     Создаёт итоговую матрицу преобразования, объединяя масштабирование, вращение и перевод.
    ///     Порядок умножения: итоговая матрица = Translation * Rotation * Scale.
    ///     В данном случае векторы представляют как столбцы (OpenGL)
    /// </summary>
    /// <param name="scale">Однородный коэффициент масштабирования или вектор масштабирования</param>
    /// <param name="rotation">Матрица вращения (можно получить, последовательно перемножая поворот вокруг осей)</param>
    /// <param name="translation">Вектор перемещения</param>
    /// <returns>Итоговая матрица преобразования 4×4</returns>
    public static Matrix4x4 CreateWorldTransform(float scale, Matrix4x4 rotation, Vector3 translation)
    {
        var scaleMatrix = Matrix4x4.CreateScale(scale);

        var translationMatrix = Matrix4x4.CreateTranslation(translation);

        var worldMatrix = rotation * translationMatrix * scaleMatrix;

        return worldMatrix;
    }

    /// <summary>
    ///     Создаёт матрицу преобразования из мирового пространства в пространство наблюдателя (view space).
    /// </summary>
    /// <param name="eye">Позиция камеры в мировом пространстве</param>
    /// <param name="target">Цель, на которую направлена камера</param>
    /// <param name="up">Вектор, указывающий направление «вверх» с точки зрения камеры</param>
    /// <returns>Матрица вида (view matrix) 4×4</returns>
    public static Matrix4x4 CreateViewMatrix(Vector3 eye, Vector3 target, Vector3 up)
    {
        // Вычисляем базис камеры
        var zAxis = Vector3.Normalize(eye - target); // Направлена от цели к камере
        var xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis)); // Перпендикулярна up и zAxis
        // var yAxis = up; // Обычно up уже нормализован (иначе можно нормализовать yAxis) (У нас не нормализован из-за сферических координат Eye)
        var yAxis = Vector3.Cross(zAxis, xAxis);

        // Вычисляем сдвиги: отрицательные скалярные произведения базисов на позицию камеры.
        var tx = -Vector3.Dot(xAxis, eye);
        var ty = -Vector3.Dot(yAxis, eye);
        var tz = -Vector3.Dot(zAxis, eye);

        // Формируем матрицу вида (должна быть инверсирована из-за конструктора для векторов-столбцов):
        var view = new Matrix4x4(
            xAxis.X, xAxis.Y, xAxis.Z, tx,
            yAxis.X, yAxis.Y, yAxis.Z, ty,
            zAxis.X, zAxis.Y, zAxis.Z, tz,
            0.0f, 0.0f, 0.0f, 1.0f);

        view = Matrix4x4.Transpose(view);

        return view;
    }

    /// <summary>
    ///     Создаёт матрицу перспективной проекции, используя поле зрения по оси Y.
    ///     Результирующая матрица переводит координаты из пространства наблюдателя
    ///     в каноническое пространство проекции.
    /// </summary>
    /// <param name="fov">Угол поля зрения по оси Y (в радианах)</param>
    /// <param name="aspect">Соотношение сторон (ширина/высота)</param>
    /// <param name="znear">Расстояние до ближней плоскости обзора</param>
    /// <param name="zfar">Расстояние до дальней плоскости обзора</param>
    public static Matrix4x4 CreatePerspectiveProjection(float fov, float aspect, float znear, float zfar)
    {
        var tanHalfFov = MathF.Tan(fov / 2);
        // Единичный масштаб по Y равен 1/tan(fov/2). По X – делим дополнительно на aspect.
        var m00 = 1 / (aspect * tanHalfFov);
        var m11 = 1 / tanHalfFov;
        // Здесь выбирается формула, дающая z-координату в диапазоне [0, 1].
        var m22 = zfar / (znear - zfar);
        var m32 = znear * zfar / (znear - zfar);

        var perspective = new Matrix4x4(
            m00, 0, 0, 0,
            0, m11, 0, 0,
            0, 0, m22, m32,
            0, 0, -1, 0
        );

        // Надо инверсировать из-за конструктора для векторов-столбцов
        perspective = Matrix4x4.Transpose(perspective);

        return perspective;
    }

    /// <summary>
    ///     Создаёт матрицу преобразования из пространства проекции (NDC) в окно просмотра.
    ///     Преобразование масштабирует координаты в диапазоне [-1, 1] по X и Y в окно,
    ///     где ось Y перевёрнута (из-за того, что начало координат экрана – верхний левый угол).
    /// </summary>
    /// <param name="width">Ширина окна просмотра</param>
    /// <param name="height">Высота окна просмотра</param>
    /// <param name="xMin">Смещение по оси X окна (например, 0)</param>
    /// <param name="yMin">Смещение по оси Y окна (например, 0)</param>
    /// <returns>Матрица 4x4 для преобразования в координаты окна просмотра</returns>
    public static Matrix4x4 CreateViewportMatrix(float width, float height, float xMin = 0.0f, float yMin = 0.0f)
    {
        var viewportMatrix = new Matrix4x4(
            width / 2, 0, 0, xMin + width / 2,
            0, -height / 2, 0, yMin + height / 2,
            0, 0, 1, 0,
            0, 0, 0, 1
        );

        // Инверсировать из-за конструктора для векторов-столбцов
        viewportMatrix = Matrix4x4.Transpose(viewportMatrix);

        return viewportMatrix;
    }
    
    
}