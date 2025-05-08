using System.Numerics;
using AKG.Core.Shadows;

namespace AKG.Core.Objects;

/// <summary>
///     Класс модели, содержащей списки всех элементов
/// </summary>
public class ObjModel
{
    public string ModelName { get; set; } = "Unnamed Model";

    private float _scale;

    // Список исходных (оригинальных) вершин, полученных из файла OBJ.
    // V
    // W – Дополнительная координата, по умолчанию 1
    public List<Vector4> OriginalVertices { get; } = [];

    // Vt
    // V – Необязательная координата для двухмерной текстуры, по умолчанию 0
    // W – Необязательная координата для трехмерной текстуры, по умолчанию 0
    public List<Vector3> TextureCoords { get; } = [];

    // Vn
    // I – X
    // J – Y
    // K – Z
    public List<Vector3> Normals { get; } = [];

    /// <summary>
    ///     Список вершин, которые будут использоваться для отображения (после применения преобразований).
    /// </summary>
    public Vector4[] TransformedVertices { get; set; } = [];

    public Vector3[] WorldVertices { get; set; } = [];

    /// <summary>
    ///     Счетчики количества граней, использующих каждую вершину.
    /// </summary>
    public int[] Counters { get; set; } = [];

    /// <summary>
    ///     Нормали вершин (рассчитываются путем усреднения нормалей граней).
    /// </summary>
    public Vector3[] VertexNormals { get; set; } = [];

    // F/V/N список полигонов/граней
    public List<Face> Faces { get; } = [];
    public float[] WValues { get; set; } = [];

    // Словарь материалов
    public Dictionary<string, Material>? Materials { get; set; }

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
        }
    }

    // Смещение модели
    public Vector3 Translation { get; set; } = Vector3.Zero;

    // Вращение модели (углы в радианах по осям X, Y, Z).
    public Vector3 Rotation { get; set; } = Vector3.Zero;
    
    public Matrix4x4 WorldMatrix { get; private set; }

    // Дополнительная величина, например, для шага перемещения
    public float Delta { get; set; }

    /// <summary>
    ///     Рассчитывает нормали вершин на основе нормалей граней.
    /// </summary>
    public void CalculateVertexNormals()
    {
        // Инициализируем нормали и счетчики нулями
        for (var i = 0; i < OriginalVertices.Count; i++)
        {
            VertexNormals[i] = Vector3.Zero;
            Counters[i] = 0;
        }

        // Для каждой грани выполняем фан-трайангуляцию
        Parallel.ForEach(Faces, face =>
        {
            if (face.Vertices.Count < 3)
                return;

            for (var j = 1; j < face.Vertices.Count - 1; j++)
            {
                var idx0 = face.Vertices[0].VertexIndex - 1;
                var idx1 = face.Vertices[j].VertexIndex - 1;
                var idx2 = face.Vertices[j + 1].VertexIndex - 1;

                if (idx0 < 0 || idx1 < 0 || idx2 < 0 ||
                    idx0 >= OriginalVertices.Count || idx1 >= OriginalVertices.Count || idx2 >= OriginalVertices.Count)
                    continue;

                // Преобразуем исходные вершины с учетом текущей мировой матрицы
                var worldV0 = WorldVertices[idx0];
                var worldV1 = WorldVertices[idx1];
                var worldV2 = WorldVertices[idx2];

                // Проверяем на вырожденность треугольника
                if (worldV0 == worldV1 || worldV1 == worldV2 || worldV0 == worldV2)
                    continue;

                // Вычисляем нормаль данного треугольника
                var edge1 = worldV1 - worldV0;
                var edge2 = worldV2 - worldV0;
                var triNormal = Vector3.Cross(edge1, edge2);

                // Проверяем, что нормаль не является нулевой
                if (triNormal.LengthSquared() > float.Epsilon)
                {
                    triNormal = Vector3.Normalize(triNormal);

                    // Добавляем нормаль треугольника к каждой из вершин
                    AddFaceNormalToVertex(idx0, triNormal);
                    AddFaceNormalToVertex(idx1, triNormal);
                    AddFaceNormalToVertex(idx2, triNormal);
                }
            }
        });

        // Усредняем нормали для каждой вершины
        Parallel.For(0, VertexNormals.Length, i =>
        {
            if (Counters[i] > 0) VertexNormals[i] = Vector3.Normalize(VertexNormals[i] / Counters[i]);
        });

        void AddFaceNormalToVertex(int idx, Vector3 normal)
        {
            VertexNormals[idx] += normal;
            Counters[idx]++;
        }
    }

    public Vector3 GetOptimalTranslationStep()
    {
        var dx = Max.X - Min.X;
        var dy = Max.Y - Min.Y;
        var dz = Max.Z - Min.Z;

        var stepX = dx / 50.0f;
        var stepY = dy / 50.0f;
        var stepZ = dz / 50.0f;

        return new Vector3(stepX, stepY, stepZ);
    }

    /// <summary>
    ///     Применяет преобразование к вершинам модели после перемножений матриц World x View x Projection x Viewport
    /// </summary>
    /// <param name="camera">Камера, которой смотрят на модель</param>
    /// <param name="worldTransform">Матрица мировая</param>
    /// <param name="finalTransform">Матрица, финального преобразования</param>
    public void ApplyFinalTransformation(Matrix4x4 worldTransform, Matrix4x4 finalTransform, Camera camera)
    {
        var count = OriginalVertices.Count;
        WorldMatrix = worldTransform;
        Parallel.For(0, count, i =>
        {
            var v = Vector4.Transform(OriginalVertices[i], worldTransform);

            WorldVertices[i] = v.AsVector3();

            v = Vector4.Transform(v, finalTransform);

            WValues[i] = v.W;

            if (v.W > camera.ZNear && v.W < camera.ZFar) v /= v.W;

            /*if (v.W != 0)
            {
                v /= v.W;
            }*/

            TransformedVertices[i] = v;
        });
    }

    public BvhNode? BvhTree { get; set; }
}