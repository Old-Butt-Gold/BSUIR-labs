using System.Numerics;
using AKG.Core.Parser;

namespace AKG.Core.VectorTransformations;

public static class Transformations
{
    // !!!! ВАЖНО, конструкторы должны быть инверсированы для формул !!!!
    
    /// <summary>
    /// Создаёт итоговую матрицу преобразования, объединяя масштабирование, вращение и перевод.
    /// Порядок умножения: итоговая матрица = Translation * Rotation * Scale.
    /// В данном случае векторы представляют как столбцы (OpenGL)
    /// </summary>
    /// <param name="scale">Однородный коэффициент масштабирования или вектор масштабирования</param>
    /// <param name="rotation">Матрица вращения (можно получить, последовательно перемножая поворот вокруг осей)</param>
    /// <param name="translation">Вектор перемещения</param>
    /// <returns>Итоговая матрица преобразования 4×4</returns>
    public static Matrix4x4 CreateWorldTransform(float scale, Matrix4x4 rotation, Vector3 translation)
    {
        // Если нужен равномерный масштаб:
        var scaleMatrix = Matrix4x4.CreateScale(scale);

        // Матрица перемещения:
        var translationMatrix = Matrix4x4.CreateTranslation(translation);

        // Итоговая матрица (порядок: сначала масштаб, затем вращение, затем перевод)
        // Если представлять вершину в виде столбца (как у нас и в OpenGL), то итоговое преобразование: M = T * R * S.
        var worldMatrix = translationMatrix * rotation * scaleMatrix; // сначала T, потом R, затем S
        
        return worldMatrix;
        
        /*Matrix4x4 rotation = Matrix4x4.CreateRotationY(MathF.PI / 2);  // 90° = PI/2 радиан
        Matrix4x4 worldTransform = Transformations.CreateWorldTransform(model.Scale, rotation, new Vector3(0, 0, 10));
        Transformations.ApplyTransformation(model, worldTransform);*/
    }
    
    /// <summary>
    /// Создаёт матрицу преобразования из мирового пространства в пространство наблюдателя (view space).
    /// </summary>
    /// <param name="eye">Позиция камеры в мировом пространстве</param>
    /// <param name="target">Цель, на которую направлена камера</param>
    /// <param name="up">Вектор, указывающий направление «вверх» с точки зрения камеры</param>
    /// <returns>Матрица вида (view matrix) 4×4</returns>
    public static Matrix4x4 CreateViewMatrix(Vector3 eye, Vector3 target, Vector3 up)
    {
        // аналог метода Matrix4x4.CreateLookAt:
        // eye – cameraPosition
        // target – cameraTarget
        // up – cameraUpVector
        
        // Вычисляем базис камеры
        var zAxis = Vector3.Normalize(eye - target);  // Направлена от цели к камере
        var xAxis = Vector3.Normalize(Vector3.Cross(up, zAxis)); // Перпендикулярна up и zAxis
        var yAxis = up; // Обычно up уже нормализован (иначе можно нормализовать yAxis)

        // Вычисляем сдвиги: отрицательные скалярные произведения базисов на позицию камеры.
        float tx = -Vector3.Dot(xAxis, eye);
        float ty = -Vector3.Dot(yAxis, eye);
        float tz = -Vector3.Dot(zAxis, eye);

        // Формируем матрицу вида (должна быть инверсирована из-за конструктора):
        var view = new Matrix4x4(
            xAxis.X, xAxis.Y, xAxis.Z, tx,
            yAxis.X, yAxis.Y, yAxis.Z, ty,
            zAxis.X, zAxis.Y, zAxis.Z, tz,
            0.0f,    0.0f,    0.0f,    1.0f);

        view = Matrix4x4.Transpose(view);

        return view;
    }

    /// <summary>
    /// Создаёт матрицу перспективной проекции, используя поле зрения по оси Y.
    /// Результирующая матрица переводит координаты из пространства наблюдателя
    /// в каноническое пространство проекции.
    /// </summary>
    /// <param name="fov">Угол поля зрения по оси Y (в радианах)</param>
    /// <param name="aspect">Соотношение сторон (ширина/высота)</param>
    /// <param name="znear">Расстояние до ближней плоскости обзора</param>
    /// <param name="zfar">Расстояние до дальней плоскости обзора</param>
    public static Matrix4x4 CreatePerspectiveProjection(float fov, float aspect, float znear, float zfar)
    {
        float tanHalfFov = MathF.Tan(fov / 2);
        // Единичный масштаб по Y равен 1/tan(fov/2). По X – делим дополнительно на aspect.
        float m00 = 1 / (aspect * tanHalfFov);
        float m11 = 1 / tanHalfFov;
        // Здесь выбирается формула, дающая z-координату в диапазоне [0, 1].
        float m22 = zfar / (znear - zfar);
        float m32 = (znear * zfar) / (znear - zfar);

        var perspective = new Matrix4x4(
            m00, 0,    0,   0,
            0,   m11,  0,   0,
            0,   0,    m22, m32,
            0,   0,   -1,   0
        );

        // Надо инверсировать из-за конструктора
        perspective = Matrix4x4.Transpose(perspective);

        return perspective;
    }
    
    /// <summary>
    /// Создаёт матрицу преобразования из пространства проекции (NDC) в окно просмотра.
    /// Преобразование масштабирует координаты в диапазоне [-1, 1] по X и Y в окно,
    /// где ось Y перевёрнута (из-за того, что начало координат экрана – верхний левый угол).
    /// </summary>
    /// <param name="width">Ширина окна просмотра</param>
    /// <param name="height">Высота окна просмотра</param>
    /// <param name="xMin">Смещение по оси X окна (например, 0)</param>
    /// <param name="yMin">Смещение по оси Y окна (например, 0)</param>
    /// <returns>Матрица 4x4 для преобразования в координаты окна просмотра</returns>
    public static Matrix4x4 CreateViewportMatrix(float width, float height, float xMin = 0.0f, float yMin = 0.0f)
    {
        var viewportMatrix = new Matrix4x4(
            width / 2,  0,            0,  xMin + width / 2,
            0,         -height / 2,   0,  yMin + height / 2,
            0,          0,            1,  0,
            0,          0,            0,  1
        );

        // Инверсировать из-за конструктора
        viewportMatrix = Matrix4x4.Transpose(viewportMatrix);

        return viewportMatrix;
    }
    
    /// <summary>
    /// Применяет матричное преобразование ко всем вершинам модели.
    /// </summary>
    /// <param name="model">Модель, вершины которой необходимо преобразовать</param>
    /// <param name="transform">Матрица преобразования</param>
    public static void ApplyWorldTransformation(this ObjModel model, Matrix4x4 transform)
    {
        int count = model.OriginalVertices.Count;
        Parallel.For(0, count, i =>
        {
            // Преобразуем исходную вершину и записываем в TransformedVertices по индексу i.
            model.TransformedVertices[i] = Vector4.Transform(model.OriginalVertices[i], transform);
        });
    }
    
    /// <summary>
    /// Применяет матричное преобразование вида (view transformation) ко всем вершинам модели.
    /// </summary>
    /// <param name="model">Модель, вершины которой необходимо преобразовать</param>
    /// <param name="viewMatrix">Матрица преобразования вида</param>
    public static void ApplyViewTransformation(this ObjModel model, Matrix4x4 viewMatrix)
    {
        int count = model.OriginalVertices.Count;
        Parallel.For(0, count, i =>
        {
            model.TransformedVertices[i] = Vector4.Transform(model.TransformedVertices[i], viewMatrix);
        });
    }
    
    /// <summary>
    /// Применяет матричное преобразование координат из
    /// пространства наблюдателя в пространство проекции ко всем вершинам модели.
    /// </summary>
    public static void ApplyTransformationProjection(this ObjModel model, Matrix4x4 transform)
    {
        int count = model.OriginalVertices.Count;
        Parallel.For(0, count, i =>
        {
            var v = Vector4.Transform(model.TransformedVertices[i], transform);
            if (v.W > model.ZNear) 
            {
                v /= v.W;
            }
            
            /*if (v.W != 0)
            {
                v /= v.W;
            }*/

            model.TransformedVertices[i] = v;
        });
    }

    /// <summary>
    /// Применяет данное преобразование ко всем вершинам модели.
    /// </summary>
    /// <param name="model">Модель, вершины которой будут преобразованы</param>
    /// <param name="viewportMatrix">Матрица преобразования окна просмотра</param>
    public static void ApplyViewportTransformation(this ObjModel model, Matrix4x4 viewportMatrix)
    {
        int count = model.OriginalVertices.Count;
        Parallel.For(0, count, i =>
        {
            model.TransformedVertices[i] = Vector4.Transform(model.TransformedVertices[i], viewportMatrix);
        });
        /*for (int i = 0; i < model.OriginalVertices.Count; i++)
        {
            // Находятся в диапазоне [-1; 1], * на Height / 2, => надо + 1???
            /*var vertice = model.TransformedVertices[i];
            vertice.X = vertice.X + 1;
            vertice.Y = -vertice.Y + 1;
            model.TransformedVertices[i] = vertice;#1#

            model.TransformedVertices[i] = Vector4.Transform(model.TransformedVertices[i], viewportMatrix);
        }*/
    }

    /// <summary>
    /// Применяет преобразование к вершинам модели после перемножений матриц World x View x Projection x Viewport
    /// </summary>
    /// <param name="model">Модль, вершины которой будут преобразованы</param>
    /// <param name="finalTransform">Матрица, финального преобразования</param>
    public static void ApplyFinalTransformation(this ObjModel model, Matrix4x4 finalTransform)
    {
        int count = model.OriginalVertices.Count;
        Parallel.For(0, count, i =>
        {
            var v = Vector4.Transform(model.OriginalVertices[i], finalTransform);
            if (v.W > model.ZNear) 
            {
                v /= v.W;
            }
            
            /*if (v.W != 0)
            {
                v /= v.W;
            }*/

            model.TransformedVertices[i] = v;
        });
    }
}