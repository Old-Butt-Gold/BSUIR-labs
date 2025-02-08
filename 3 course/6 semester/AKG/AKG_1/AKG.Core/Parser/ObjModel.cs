using System.Drawing;
using System.Numerics;
using AKG.Core.VectorTransformations;

namespace AKG.Core.Parser;

/// <summary>
/// Класс модели, содержащей списки всех элементов 
/// </summary>
public class ObjModel
{
    private float _scale;
    private Vector3 _translation = Vector3.Zero;
    private Vector3 _rotation = Vector3.Zero;

    // Список исходных (оригинальных) вершин, полученных из файла OBJ.
    // V
    // W – Дополнительная координата, по умолчанию 1
    public List<Vector4> OriginalVertices { get; } = [];
    
    // Список вершин, которые будут использоваться для отображения (после применения преобразований).
    // Этот список обновляется в методе UpdateImage.
    public Vector4[] TransformedVertices { get; set; } = [];

    // Vt
    // V – Необязательная координата для двухмерной текстуры, по умолчанию 0
    // W – Необязательная координата для трехмерной текстуры, по умолчанию 0
    public List<Vector3> TextureCoords { get; } = [];
    // Vn
    // I – X
    // J – Y
    // K – Z
    public List<Vector3> Normals { get; } = [];
    
    // F/V/N список полигонов/граней
    public List<Face> Faces { get; } = [];
    
    // Bounding box (минимальные и максимальные координаты по X, Y, Z)
    public Vector4 Min { get; set; }
    public Vector4 Max { get; set; }

    // Коэффициент масштабирования, рассчитанный по размеру объекта.
    public float Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            Delta = _scale / 10.0f;
            OnTransformationChanged();
        }
    }

    // Смещение модели
    public Vector3 Translation
    {
        get => _translation;
        set
        {
            if (_translation != value)
            {
                _translation = value;
                OnTransformationChanged();
            }
        }
    }

    // Вращение модели (углы в радианах по осям X, Y, Z).
    public Vector3 Rotation
    {
        get => _rotation;
        set
        {
            if (_rotation != value)
            {
                _rotation = value;
                OnTransformationChanged();
            }
        }
    }

    // Дополнительная величина, например, для шага перемещения
    public float Delta { get; set; }
    
    //Размер экрана
    public Size WindowSize { get; set; }

    // Параметры камеры:
    
    // Позиция камеры в мировом пространстве
    public Vector3 Eye { get; init; } = new(1.0f, 1.0f, MathF.PI);
    
    // Позиция цели, на которую направлена камера
    // направлена в центр сцены
    public Vector3 Target { get; init; } = Vector3.Zero;
    
    // Вектор, направленный вертикально вверх с точки зрения камеры
    // Вектор вверх (ось Y)
    public Vector3 Up { get; init; } = Vector3.UnitY;
    
    // Поле зрения камеры по оси Y (в радианах)
    public float Fov { get; init; } = MathF.PI / 4.0f; // 45° = PI / 4
    
    // Соотношение сторон обзора камеры
    public float Aspect { get; init; } = 16f / 9f;

    // Расстояние до ближней плоскости обзора
    public float ZNear { get; init; } = 1f; // было 0.01f, при приближении проблемы возникают
    
    // Расстояние до дальней плоскости обзора
    public float ZFar { get; init; } = 100.0f;
    
    /// <summary>
    /// Событие, которое вызывается при изменении параметров трансформации.
    /// На него можно подписаться, чтобы, например, перерисовать изображение.
    /// </summary>
    public event EventHandler? TransformationChanged;

    /// <summary>
    /// Вызывается при изменении Scale, Translation или Rotation.
    /// Пересчитывает матрицы и обновляет трансформированные вершины, а затем генерирует событие.
    /// </summary>
    protected virtual void OnTransformationChanged()
    {
        UpdateImage();
        TransformationChanged?.Invoke(this, EventArgs.Empty);
    }
    
    /// <summary>
    /// Обновляет отображаемые (трансформированные) вершины.
    /// Исходно копирует данные из OriginalVertices, затем последовательно
    /// применяет преобразования: мировое -> вид -> проекция -> viewport.
    /// </summary>
    public void UpdateImage()
    {
        // Start point to change TransformedVertices
        var rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z);
        var worldTransform = Transformations.CreateWorldTransform(Scale, rotationMatrix, Translation);
        //this.ApplyWorldTransformation(worldTransform);

        var viewTransform = Transformations.CreateViewMatrix(Eye, Target, Up);
        //this.ApplyViewTransformation(viewTransform);

        var projectionTransform = Transformations.CreatePerspectiveProjection(Fov, Aspect, ZNear, ZFar);
        //this.ApplyTransformationProjection(projectionTransform);

        var viewportTransform = Transformations.CreateViewportMatrix(WindowSize.Width, WindowSize.Height);
        //this.ApplyViewportTransformation(viewportTransform);
        
        var finalTransform = worldTransform * viewTransform * projectionTransform * viewportTransform;
        this.ApplyFinalTransformation(finalTransform);
    }

    public Vector3 GetOptimalTranslationStep()
    {
        float dx = Max.X - Min.X;
        float dy = Max.Y - Min.Y;
        float dz = Max.Z - Min.Z;

        float stepX = dx / 50.0f;
        float stepY = dy / 50.0f;
        float stepZ = dz / 50.0f;

        return new Vector3(stepX, stepY, stepZ);
    }
}