using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using AKG.Core.VectorTransformations;

namespace AKG.Core.Objects;

public class Scene
{
    // Список моделей, отображаемых на холсте.
    public List<ObjModel> Models { get; } = [];

    // Список лучей/точек света, падающих на 3D объект
    public List<Light> Lights { get; } = [];

    // Камера для сцены.
    public Camera Camera { get; set; } = new();

    // Размеры холста (например, размер WriteableBitmap).
    public int CanvasWidth { get; set; }
    public int CanvasHeight { get; set; }

    public ObjModel? SelectedModel { get; set; }

    public HdRiBackground? Background { get; set; }

    public Matrix4x4 GetViewportMatrix() => Transformations.CreateViewportMatrix(CanvasWidth, CanvasHeight);

    public void UpdateAllModels()
    {
        var view = Camera.GetViewMatrix();
        var projection = Camera.GetProjectionMatrix();
        var viewport = GetViewportMatrix();

        foreach (var model in Models)
        {
            // Вычисляем мировую матрицу для модели на основе её локальных параметров:
            var world = Transformations.CreateWorldTransform(
                model.Scale,
                Matrix4x4.CreateFromYawPitchRoll(model.Rotation.Y, model.Rotation.X, model.Rotation.Z),
                model.Translation);

            // Композиция матриц: World * View * Projection * Viewport
            var finalTransform = view * projection * viewport;
            model.ApplyFinalTransformation(world, finalTransform, Camera);
        }
    }

    public ObjModel? PickModel(Point clickPoint)
    {
        ObjModel? pickedModel = null;
        var bestDepth = float.MaxValue;

        foreach (var model in Models)
        {
            if (model.TransformedVertices.Length == 0)
                continue;

            // Вычисляем экранный bounding box для модели
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            var modelDepth = float.MaxValue;

            foreach (var v in model.TransformedVertices)
            {
                minX = MathF.Min(minX, v.X);
                minY = MathF.Min(minY, v.Y);
                maxX = MathF.Max(maxX, v.X);
                maxY = MathF.Max(maxY, v.Y);
                // Используем минимальное Z (ближайшую к камере точку)
                modelDepth = MathF.Min(modelDepth, v.Z);
            }

            // Проверяем, находится ли точка клика внутри bounding box
            if (clickPoint.X >= minX && clickPoint.X <= maxX &&
                clickPoint.Y >= minY && clickPoint.Y <= maxY)
                // Если моделей несколько, выбираем ту, которая ближе к камере (меньший Z)
                if (modelDepth < bestDepth)
                {
                    bestDepth = modelDepth;
                    pickedModel = model;
                }
        }

        return pickedModel;
    }

    public Light? PickLight(Point clickPoint, Image image, float radius = 10.0f)
    {
        Light? pickedLight = null;     
        var bestDepth = float.MaxValue;

        // Преобразуем координаты клика из системы координат окна в систему координат Image
        var imageClickPoint = image.TransformToVisual(Application.Current.MainWindow!)
            .Transform(new Point(0, 0));
        imageClickPoint = new Point(clickPoint.X - imageClickPoint.X, clickPoint.Y - imageClickPoint.Y);

        foreach (var light in Lights)
        {
            // Преобразуем позицию источника света в экранные координаты
            var lightPosition = light.Position;
            var view = Camera.GetViewMatrix();
            var projection = Camera.GetProjectionMatrix();
            var viewport = GetViewportMatrix();

            var transformedPosition = Vector4.Transform(new Vector4(lightPosition, 1.0f), view * projection * viewport);
            if (transformedPosition.W != 0)
                transformedPosition /= transformedPosition.W;

            var screenPosition = new Point(transformedPosition.X, transformedPosition.Y);

            // Проверяем, находится ли курсор в пределах радиуса от позиции источника света
            if (Math.Abs(imageClickPoint.X - screenPosition.X) <= radius &&
                Math.Abs(imageClickPoint.Y - screenPosition.Y) <= radius)
            {
                // Используем Z-координату для определения ближайшего источника света
                if (transformedPosition.Z < bestDepth)
                {
                    bestDepth = transformedPosition.Z;
                    pickedLight = light;
                }
            }
        }

        return pickedLight;
    }
}