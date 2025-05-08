using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AKG.Core.Extensions;
using AKG.Core.ImageHelpers;
using AKG.Core.Objects;
using AKG.Core.Parser;
using AKG.Core.Shadows;
using AKG.Core.VectorTransformations;

namespace AKG.Core.Renderer;

public static class Rasterizer
{
    // Z-буфер: хранит глубину для каждого пикселя; 
    private static float[,]? _zBuffer;

    public static void ClearZBuffer(int width, int height, Camera camera)
    {
        _zBuffer ??= new float[width, height];
        var initDepth = camera.ZFar;
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                _zBuffer[x, y] = initDepth;
            }
        }
    }

    public static unsafe void DrawFilledTriangleLambert(ObjModel model, WriteableBitmap wb, Color color, Camera camera,
        List<Light> lights)
    {
        var width = wb.PixelWidth;
        var height = wb.PixelHeight;

        wb.Lock();

        var buffer = (int*)wb.BackBuffer;

        // Для каждой грани модели
        Parallel.ForEach(model.Faces, face =>
        {
            if (face.Vertices.Count < 3) return;

            //Если грань содержит больше 3 вершин, выполняем трайангуляцию
            for (var j = 1; j < face.Vertices.Count - 1; j++)
            {
                var idx0 = face.Vertices[0].VertexIndex - 1;
                var idx1 = face.Vertices[j].VertexIndex - 1;
                var idx2 = face.Vertices[j + 1].VertexIndex - 1;

                if (idx0 < 0 || idx1 < 0 || idx2 < 0 ||
                    idx0 >= model.TransformedVertices.Length ||
                    idx1 >= model.TransformedVertices.Length ||
                    idx2 >= model.TransformedVertices.Length)
                    continue;

                //Вычисляем нормаль треугольника в мировых координатах
                var worldV0 = model.WorldVertices[idx0];
                var worldV1 = model.WorldVertices[idx1];
                var worldV2 = model.WorldVertices[idx2];

                var edge1 = worldV1 - worldV0;
                var edge2 = worldV2 - worldV0;
                
                var normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

                // Backface culling: если треугольник обращён от камеры, отбраковываем грань
                var viewDirection = worldV0 - camera.Eye; // Вектор взгляда от камеры к вершине
                if (Vector3.Dot(normal, viewDirection) > 0)
                    continue; // Если скалярное произведение положительное, грань отвернута

                // Расчет интенсивности освещения по модели Ламберта
                var shadedColor = Light.ApplyLambert(lights, normal, color);

                // Получаем экранные координаты (после всех преобразований)
                var screenV0 = model.TransformedVertices[idx0].AsVector3();
                var screenV1 = model.TransformedVertices[idx1].AsVector3();
                var screenV2 = model.TransformedVertices[idx2].AsVector3();

                if ((screenV0.X >= width && screenV1.X >= width && screenV2.X >= width)
                    || (screenV0.X <= 0 && screenV1.X <= 0 && screenV2.X <= 0)
                    || (screenV0.Y >= height && screenV1.Y >= height && screenV2.Y >= height)
                    || (screenV0.Y <= 0 && screenV1.Y <= 0 && screenV2.Y <= 0)
                    || screenV0.Z < camera.ZNear || screenV1.Z < camera.ZNear || screenV2.Z < camera.ZNear
                    || screenV0.Z > camera.ZFar || screenV1.Z > camera.ZFar || screenV2.Z > camera.ZFar)
                    continue;

                // Растеризуем треугольник с заливкой и Z-тестом
                DrawFilledTriangleLambert(screenV0, screenV1, screenV2, shadedColor, buffer, width, height);
            }
        });

        wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
        wb.Unlock();
    }

    /// <summary>
    ///     Растеризует (заполняет) один треугольник, заданный тремя вершинами в экранном пространстве.
    ///     Метод использует сканирующую линию с вычислением барицентрических координат для интерполяции глубины.
    ///     Отбраковка невидимых фрагментов осуществляется с помощью Z-буфера.
    /// </summary>
    private static unsafe void DrawFilledTriangleLambert(Vector3 v0, Vector3 v1, Vector3 v2, Color color, int* buffer,
        int width, int height)
    {
        // Определяем ограничивающий прямоугольник (обрамлены Math.Max и Math.Min, чтобы не уходили за экран)
        var minX = Math.Max(0, (int)Math.Floor(Math.Min(v0.X, Math.Min(v1.X, v2.X))));
        var maxX = Math.Min(width - 1, (int)Math.Ceiling(Math.Max(v0.X, Math.Max(v1.X, v2.X))));
        var minY = Math.Max(0, (int)Math.Floor(Math.Min(v0.Y, Math.Min(v1.Y, v2.Y))));
        var maxY = Math.Min(height - 1, (int)Math.Ceiling(Math.Max(v0.Y, Math.Max(v1.Y, v2.Y))));

        // Вычисляем знаменатель барицентрических координат
        var denom = (v1.Y - v2.Y) * (v0.X - v2.X) + (v2.X - v1.X) * (v0.Y - v2.Y);
        if (Math.Abs(denom) < float.Epsilon) return; // Вырожденный треугольник

        var invDenom = 1.0f / denom;

        for (var y = minY; y <= maxY; y++)
        {
            if (y < 0 || y >= height)
                return;

            for (var x = minX; x <= maxX; x++)
            {
                if (x < 0 || x >= width)
                    continue;

                // Вычисляем барицентрические координаты: alpha, beta, gamma
                var alpha = ((v1.Y - v2.Y) * (x - v2.X) + (v2.X - v1.X) * (y - v2.Y)) * invDenom;
                var beta = ((v2.Y - v0.Y) * (x - v2.X) + (v0.X - v2.X) * (y - v2.Y)) * invDenom;
                var gamma = 1 - alpha - beta;

                // Если точка внутри треугольника (включая границы)
                if (alpha >= 0 && beta >= 0 && gamma >= 0)
                {
                    // Интерполируем глубину по барицентрическим координатам
                    var depth = alpha * v0.Z + beta * v1.Z + gamma * v2.Z;
                    // Если новый фрагмент ближе (меньшее значение depth) – обновляем Z-буфер и рисуем пиксель
                    if (depth < _zBuffer![x, y])
                    {
                        _zBuffer[x, y] = depth;
                        buffer[y * width + x] = color.ColorToIntBgra();
                    }
                }
            }
        }
    }

    // Поддерживаются два режима:
    // FilledTrianglesPhong – вычисление цвета на уровне пикселя (обычное Фонговое затенение)
    // FilledTrianglesAverageFaceNormalPhong – использование усреднённых нормалей вершин (Гуравское затенение)

    /// <summary>
    ///     Растеризует треугольники для каждой грани модели с применением фан-трайангуляции, backface culling и модели Фонга.
    ///     Для каждой треугольной части вычисляются экранные координаты и, если треугольник видим (с учетом нормали),
    ///     происходит заполнение с использованием Z-буфера и вычислением цвета по модели Фонга.
    /// </summary>
    public static unsafe void DrawFilledTrianglePhong(ObjModel model, WriteableBitmap wb,
        Camera camera, List<Light> lights, bool normalsFromFile)
    {
        var width = wb.PixelWidth;
        var height = wb.PixelHeight;
        
        wb.Lock();
        var buffer = (int*)wb.BackBuffer;

        if (!normalsFromFile)
        {
            model.CalculateVertexNormals();
        }

        // Для каждой грани модели (фан-трайангуляция)
        Parallel.ForEach(model.Faces, face =>
        {
            if (face.Vertices.Count < 3) return;

            // Для каждой треугольной части грани
            for (var j = 1; j < face.Vertices.Count - 1; j++)
            {
                var idx0 = face.Vertices[0].VertexIndex - 1;
                var idx1 = face.Vertices[j].VertexIndex - 1;
                var idx2 = face.Vertices[j + 1].VertexIndex - 1;

                if (idx0 < 0 || idx1 < 0 || idx2 < 0 ||
                    idx0 >= model.TransformedVertices.Length ||
                    idx1 >= model.TransformedVertices.Length ||
                    idx2 >= model.TransformedVertices.Length)
                    continue;

                // Вычисляем мировые координаты вершин (для backface culling)
                var worldV0 = model.WorldVertices[idx0];
                var worldV1 = model.WorldVertices[idx1];
                var worldV2 = model.WorldVertices[idx2];

                // Вычисляем нормаль треугольника (в мировых координатах)
                var edge1 = worldV1 - worldV0;
                var edge2 = worldV2 - worldV0;
                var faceNormal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

                // Backface culling: если треугольник обращён от камеры, отбраковываем грань
                var viewDirection = worldV0 - camera.Eye; // Вектор взгляда от камеры к вершине
                if (Vector3.Dot(faceNormal, viewDirection) > 0)
                    continue; // Если скалярное произведение положительное, грань отвернута

                // Получаем экранные координаты (уже после всех преобразований)
                var screenV0 = model.TransformedVertices[idx0].AsVector3();
                var screenV1 = model.TransformedVertices[idx1].AsVector3();
                var screenV2 = model.TransformedVertices[idx2].AsVector3();

                if ((screenV0.X >= width && screenV1.X >= width && screenV2.X >= width)
                    || (screenV0.X <= 0 && screenV1.X <= 0 && screenV2.X <= 0)
                    || (screenV0.Y >= height && screenV1.Y >= height && screenV2.Y >= height)
                    || (screenV0.Y <= 0 && screenV1.Y <= 0 && screenV2.Y <= 0)
                    || screenV0.Z < camera.ZNear || screenV1.Z < camera.ZNear || screenV2.Z < camera.ZNear
                    || screenV0.Z > camera.ZFar || screenV1.Z > camera.ZFar || screenV2.Z > camera.ZFar)
                    continue;

                Vector3 n0, n1, n2;

                if (normalsFromFile)
                {
                    // Определяем нормали для затенения:
                    // Если в модели заданы нормали для вершин, используем их; иначе – используем нормаль грани.
                    n0 = face.Vertices[0].NormalIndex > 0
                        ? Vector3.TransformNormal(model.Normals[face.Vertices[0].NormalIndex - 1], model.WorldMatrix)
                        : faceNormal;
                    n1 = face.Vertices[j].NormalIndex > 0
                        ? Vector3.TransformNormal(model.Normals[face.Vertices[j].NormalIndex - 1], model.WorldMatrix)
                        : faceNormal;
                    n2 = face.Vertices[j + 1].NormalIndex > 0
                        ? Vector3.TransformNormal(model.Normals[face.Vertices[j + 1].NormalIndex - 1],
                            model.WorldMatrix)
                        : faceNormal;
                }
                else
                {
                    n0 = model.VertexNormals[idx0];
                    n1 = model.VertexNormals[idx1];
                    n2 = model.VertexNormals[idx2];
                }

                // Отрисовываем треугольник с Фонговым затенением.
                DrawFilledTrianglePhong(screenV0, screenV1, screenV2,
                    n0, n1, n2, worldV0, worldV1, worldV2,
                    buffer, width, height, lights, camera);
            }
        });

        wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
        wb.Unlock();
    }

    /// <summary>
    ///     Растеризует один треугольник с Фонговым затенением.
    ///     Для каждого пикселя внутри ограничивающего прямоугольника вычисляются барицентрические координаты,
    ///     интерполируется глубина, а также мировая позиция и нормаль, после чего вычисляется итоговый цвет фрагмента по
    ///     модели Фонга.
    ///     Отбраковка невидимых фрагментов производится с помощью Z-буфера.
    /// </summary>
    private static unsafe void DrawFilledTrianglePhong(Vector3 v0, Vector3 v1, Vector3 v2,
        Vector3 n0, Vector3 n1, Vector3 n2, Vector3 w0, Vector3 w1, Vector3 w2,
        int* buffer, int width, int height, List<Light> lights, Camera camera)
    {
        // Ограничивающий прямоугольник (не выходит за пределы экрана)
        var minX = Math.Max(0, (int)Math.Floor(Math.Min(v0.X, Math.Min(v1.X, v2.X))));
        var maxX = Math.Min(width - 1, (int)Math.Ceiling(Math.Max(v0.X, Math.Max(v1.X, v2.X))));
        var minY = Math.Max(0, (int)Math.Floor(Math.Min(v0.Y, Math.Min(v1.Y, v2.Y))));
        var maxY = Math.Min(height - 1, (int)Math.Ceiling(Math.Max(v0.Y, Math.Max(v1.Y, v2.Y))));

        // Вычисляем знаменатель барицентрических координат
        var denom = (v1.Y - v2.Y) * (v0.X - v2.X) + (v2.X - v1.X) * (v0.Y - v2.Y);
        if (Math.Abs(denom) < float.Epsilon) return; // Вырожденный треугольник
        var invDenom = 1.0f / denom;

        for (var y = minY; y <= maxY; y++)
        for (var x = minX; x <= maxX; x++)
        {
            // Вычисляем барицентрические координаты: alpha, beta, gamma
            var alpha = ((v1.Y - v2.Y) * (x - v2.X) + (v2.X - v1.X) * (y - v2.Y)) * invDenom;
            var beta = ((v2.Y - v0.Y) * (x - v2.X) + (v0.X - v2.X) * (y - v2.Y)) * invDenom;
            var gamma = 1 - alpha - beta;

            // Если точка внутри треугольника (включая границы)
            if (alpha >= 0 && beta >= 0 && gamma >= 0)
            {
                // Интерполируем глубину
                var depth = alpha * v0.Z + beta * v1.Z + gamma * v2.Z;
                // Z-тест: если новый фрагмент ближе, обновляем Z-буфер и цвет пикселя
                if (depth < _zBuffer![x, y])
                {
                    _zBuffer[x, y] = depth;

                    // Интерполируем нормаль: линейная интерполяция нормалей вершин
                    var interpNormal = Vector3.Normalize(alpha * n0 + beta * n1 + gamma * n2);

                    // Интерполируем мировую позицию фрагмента (для расчёта вектора взгляда)
                    var fragWorld = alpha * w0 + beta * w1 + gamma * w2;

                    // Вектор от фрагмента к камере.
                    // Нормализация нужна для расчета зеркальной составляющей.
                    var viewDirection = Vector3.Normalize(camera.Eye - fragWorld);

                    var lightParamers = LightParameters.DefaultLightParameters;

                    buffer[y * width + x] =
                        Light.ApplyPhongShading(lights, interpNormal, viewDirection, fragWorld,
                            lightParamers).ToColor().ColorToIntBgra();
                }
            }
        }
    }

    /// <summary>
    ///     Объединённый метод, который для каждой грани модели (с фан‑трайангуляцией)
    ///     вычисляет необходимые параметры и затем для каждого треугольника выполняет
    ///     наложение текстур: диффузной карты, карты нормалей и зеркальной карты.
    /// </summary>
    public static unsafe void DrawTexturedTriangles(ObjModel model, WriteableBitmap wb, Scene scene, bool rayTrace)
    {
        var width = wb.PixelWidth;
        var height = wb.PixelHeight;

        var camera = scene.Camera;

        if (rayTrace)
        {
            foreach (var sceneModel in scene.Models)
            {
                CreateBvh(sceneModel);
            }
        }
        
        wb.Lock();
        var buffer = (int*)wb.BackBuffer;

        // 2. Проходим по каждой грани модели (с фан‑трайангуляцией)
        Parallel.ForEach(model.Faces, face =>
        {
            if (face.Vertices.Count < 3) return;

            // Фан‑трайангуляция: для каждой грани разбиваем её на треугольники,
            // используя первую вершину и пары последовательных вершин
            for (var j = 1; j < face.Vertices.Count - 1; j++)
            {
                var idx0 = face.Vertices[0].VertexIndex - 1;
                var idx1 = face.Vertices[j].VertexIndex - 1;
                var idx2 = face.Vertices[j + 1].VertexIndex - 1;

                if (idx0 < 0 || idx1 < 0 || idx2 < 0 ||
                    idx0 >= model.TransformedVertices.Length ||
                    idx1 >= model.TransformedVertices.Length ||
                    idx2 >= model.TransformedVertices.Length)
                    continue;

                // 3. Вычисляем мировые координаты вершин
                var worldV0 = model.WorldVertices[idx0];
                var worldV1 = model.WorldVertices[idx1];
                var worldV2 = model.WorldVertices[idx2];

                // 4. Вычисляем нормаль треугольника для backface culling
                var edge1 = worldV1 - worldV0;
                var edge2 = worldV2 - worldV0;
                var faceNormal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

                // Если треугольник обращён от камеры, пропускаем его
                var viewDir = worldV0 - camera.Eye;
                if (Vector3.Dot(faceNormal, viewDir) > 0)
                    continue;

                // 5. Получаем экранные координаты вершин
                var screenV0 = model.TransformedVertices[idx0].AsVector3();
                var screenV1 = model.TransformedVertices[idx1].AsVector3();
                var screenV2 = model.TransformedVertices[idx2].AsVector3();

                // Если треугольник полностью вне экрана – пропускаем
                if ((screenV0.X >= width && screenV1.X >= width && screenV2.X >= width) ||
                    (screenV0.X <= 0 && screenV1.X <= 0 && screenV2.X <= 0) ||
                    (screenV0.Y >= height && screenV1.Y >= height && screenV2.Y >= height) ||
                    (screenV0.Y <= 0 && screenV1.Y <= 0 && screenV2.Y <= 0) ||
                    screenV0.Z < camera.ZNear || screenV1.Z < camera.ZNear || screenV2.Z < camera.ZNear ||
                    screenV0.Z > camera.ZFar || screenV1.Z > camera.ZFar || screenV2.Z > camera.ZFar)
                    continue;

                // 6. Извлекаем UV-координаты для каждой вершины
                var uv0 = model.TextureCoords[face.Vertices[0].TextureIndex - 1];
                uv0 /= model.WValues[face.Vertices[0].VertexIndex - 1];
                var uv1 = model.TextureCoords[face.Vertices[j].TextureIndex - 1];
                uv1 /= model.WValues[face.Vertices[j].VertexIndex - 1];
                var uv2 = model.TextureCoords[face.Vertices[j + 1].TextureIndex - 1];
                uv2 /= model.WValues[face.Vertices[j + 1].VertexIndex - 1];

                // 7. Определяем нормали для затенения (используем нормали вершин, если заданы)
                var n0 = face.Vertices[0].NormalIndex > 0
                    ? Vector3.TransformNormal(model.Normals[face.Vertices[0].NormalIndex - 1], model.WorldMatrix)
                    : faceNormal;
                var n1 = face.Vertices[j].NormalIndex > 0
                    ? Vector3.TransformNormal(model.Normals[face.Vertices[j].NormalIndex - 1], model.WorldMatrix)
                    : faceNormal;
                var n2 = face.Vertices[j + 1].NormalIndex > 0
                    ? Vector3.TransformNormal(model.Normals[face.Vertices[j + 1].NormalIndex - 1], model.WorldMatrix)
                    : faceNormal;
                
                // 8. Вызываем функцию отрисовки треугольника с наложением текстур
                DrawFilledTriangleTexture(screenV0, screenV1, screenV2, n0, n1, n2, worldV0, worldV1, worldV2, uv0, uv1,
                    uv2, buffer, width, height, scene, GetFaceMaterial(model, face), model, rayTrace);
            }
        });

        wb.AddDirtyRect(new Int32Rect(0, 0, wb.PixelWidth, wb.PixelHeight));
        wb.Unlock();
    }

    private static Material GetFaceMaterial(ObjModel model, Face face)
    {
        if (model.Materials != null &&
            model.Materials.TryGetValue(face.MaterialName, out var mat))
            return mat;
        return Material.DefaultMaterial; // Материал по умолчанию
    }

    /// <summary>
    ///     Метод, который для одного треугольника интерполирует параметры для каждого пикселя
    ///     и рассчитывает итоговый цвет с учетом наложения диффузной карты, карты нормалей и зеркальной карты.
    /// </summary>
    private static unsafe void DrawFilledTriangleTexture(Vector3 v0, Vector3 v1, Vector3 v2,
        Vector3 n0, Vector3 n1, Vector3 n2, Vector3 w0, Vector3 w1, Vector3 w2,
        Vector3 uv0, Vector3 uv1, Vector3 uv2, int* buffer, int width, int height, Scene scene, Material material, ObjModel model, bool rayTrace)
    {
        var diffuseTex = !string.IsNullOrEmpty(material.DiffuseMap) ? TextureLoader.Load(material.DiffuseMap) : null;
        var normalTex = !string.IsNullOrEmpty(material.NormalMap) ? TextureLoader.Load(material.NormalMap) : null;
        var mraoTex = !string.IsNullOrEmpty(material.MraoMap) ? TextureLoader.Load(material.MraoMap) : null;
        var emissiveTex = !string.IsNullOrEmpty(material.EmissiveMap) ? TextureLoader.Load(material.EmissiveMap) : null;
        var specularTex = !string.IsNullOrEmpty(material.SpecularMap) ? TextureLoader.Load(material.SpecularMap) : null;

        // Ограничивающий прямоугольник (не выходит за пределы экрана)
        var minX = Math.Max(0, (int)Math.Floor(Math.Min(v0.X, Math.Min(v1.X, v2.X))));
        var maxX = Math.Min(width - 1, (int)Math.Ceiling(Math.Max(v0.X, Math.Max(v1.X, v2.X))));
        var minY = Math.Max(0, (int)Math.Floor(Math.Min(v0.Y, Math.Min(v1.Y, v2.Y))));
        var maxY = Math.Min(height - 1, (int)Math.Ceiling(Math.Max(v0.Y, Math.Max(v1.Y, v2.Y))));

        var rotation = Matrix4x4.CreateFromYawPitchRoll(model.Rotation.Y, model.Rotation.X, model.Rotation.Z);

        // Вычисляем знаменатель барицентрических координат
        var denom = (v1.Y - v2.Y) * (v0.X - v2.X) + (v2.X - v1.X) * (v0.Y - v2.Y);
        if (Math.Abs(denom) < float.Epsilon) return; // Вырожденный треугольник
        var invDenom = 1.0f / denom;

        for (var y = minY; y <= maxY; y++)
        for (var x = minX; x <= maxX; x++)
        {
            // Вычисляем барицентрические координаты: alpha, beta, gamma
            var alpha = ((v1.Y - v2.Y) * (x - v2.X) + (v2.X - v1.X) * (y - v2.Y)) * invDenom;
            var beta = ((v2.Y - v0.Y) * (x - v2.X) + (v0.X - v2.X) * (y - v2.Y)) * invDenom;
            var gamma = 1 - alpha - beta;

            // Если точка внутри треугольника (включая границы)
            if (alpha >= 0 && beta >= 0 && gamma >= 0)
            {
                // Интерполируем глубину
                var depth = alpha * v0.Z + beta * v1.Z + gamma * v2.Z;
                // Z-тест: если новый фрагмент ближе, обновляем Z-буфер и цвет пикселя
                if (depth < _zBuffer![x, y])
                {
                    _zBuffer[x, y] = depth;

                    var lightParameters = new LightParameters(material);

                    // Линейная интерполяция uv
                    var uv = alpha * uv0 + beta * uv1 + gamma * uv2;

                    uv /= uv.Z;
                    
                    // Интерполируем мировую позицию фрагмента
                    var fragWorld = alpha * w0 + beta * w1 + gamma * w2;
                    // Интерполируем нормаль фрагмента
                    var interpNormal = Vector3.Normalize(alpha * n0 + beta * n1 + gamma * n2);

                    // Если задана карта нормалей, заменяем интерполированную нормаль
                    if (normalTex != null)
                    {
                        var normColor = TextureSampler.Sample(normalTex, uv.X, uv.Y);
                        var mapNormal = new Vector3(
                            normColor.R / 255f * 2f - 1f,
                            normColor.G / 255f * 2f - 1f,
                            normColor.B / 255f * 2f - 1f);
                        mapNormal = Vector3.Normalize(mapNormal);

                        // Применяем вращение модели к нормали (если требуется)
                        interpNormal = Vector3.TransformNormal(mapNormal, rotation);
                    }
                    

                    // Создаём локальные копии для диффузного и амбиентного цвета
                    if (diffuseTex != null)
                    {
                        var texColor = TextureSampler.Sample(diffuseTex, uv.X, uv.Y);
                        lightParameters.DiffuseColor = texColor.ToVector3();
                        lightParameters.AmbientColor = texColor.ToVector3();
                    }

                    // Если mrao‑текстура задана, извлекаем металлическость из R-канала,
                    // G – roughness, B – ambient occlusion (если потребуется)
                    if (mraoTex != null)
                    {
                        var mraoColor = TextureSampler.Sample(mraoTex, uv.X, uv.Y);
                    }
                    
                    // Если задана SpecularMap, заменяем статическое значение зеркальной компоненты
                    if (specularTex != null)
                    {
                        lightParameters.Ks = TextureSampler.Sample(specularTex, uv.X, uv.Y).ToVector3() / 255f;
                    }

                    // Вычисляем вектор взгляда (от фрагмента к камере)
                    // Нормализация нужна для расчета зеркальной составляющей.
                    var viewDir = Vector3.Normalize(scene.Camera.Eye - fragWorld);

                    Vector3 lighting;
                    
                    if (!rayTrace)
                    {
                        lighting = Light.ApplyPhongShading(scene.Lights, interpNormal, viewDir, fragWorld,
                            lightParameters);
                    }
                    else
                    {
                        lighting = Light.ApplyPhongShadingRayTracing(scene.Lights, interpNormal, viewDir, fragWorld,
                            lightParameters, scene.Models);
                    }

                    if (emissiveTex != null)
                    {
                        var emissive = TextureSampler.Sample(emissiveTex, uv.X, uv.Y).ToVector3();
                        lighting += emissive * lightParameters.Ke;
                    }

                    lighting = Vector3.Clamp(lighting, Vector3.Zero, new Vector3(255));

                    buffer[y * width + x] = lighting.ToColor().ColorToIntBgra();
                }
            }
        }
    }
    
    private static void CreateBvh(ObjModel model)
    {
        // Построим список треугольников для BVH на основе граней
        List<Triangle> triangles = [];
        foreach (var face in model.Faces)
        {
            if (face.Vertices.Count < 3)
                continue;
            
            // Фан-трайангуляция: каждая грань разбивается на треугольники,
            // где первая вершина фиксирована, а остальные образуют последовательные треугольники.
            for (int j = 1; j < face.Vertices.Count - 1; j++)
            {
                int idx0 = face.Vertices[0].VertexIndex - 1;
                int idx1 = face.Vertices[j].VertexIndex - 1;
                int idx2 = face.Vertices[j + 1].VertexIndex - 1;

                if (idx0 < 0 || idx1 < 0 || idx2 < 0 ||
                    idx0 >= model.OriginalVertices.Count ||
                    idx1 >= model.OriginalVertices.Count ||
                    idx2 >= model.OriginalVertices.Count)
                    continue;

                var worldV0 = model.WorldVertices[idx0];
                var worldV1 = model.WorldVertices[idx1];
                var worldV2 = model.WorldVertices[idx2];

                triangles.Add(new Triangle
                {
                    V0 = worldV0,
                    V1 = worldV1,
                    V2 = worldV2
                });
            }
        }

        model.BvhTree = BvhBuilder.BuildBvh(triangles);
    }
}