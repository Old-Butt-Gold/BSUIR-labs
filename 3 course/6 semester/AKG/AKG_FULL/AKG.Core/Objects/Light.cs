using System.Numerics;
using System.Windows.Media;
using AKG.Core.Shadows;

namespace AKG.Core.Objects;

public class Light
{
    public Light() { }
    
    public Light(Vector3 position, Vector3 color, float intensity)
    {
        Position = position;
        Color = color;
        Intensity = intensity;
    }

    /// <summary>
    /// Направление источника света (не нормализовано).
    /// </summary>
    public Vector3 Position { get; set; } = new(1, 1, 2);

    public Vector3 Color { get; set; } = Vector3.One; // Аналог Colors.White.ToVector3() / 255f

    public float Intensity { get; set; } = 1.0f;
    
    public static Vector3 ApplyPhongShading(List<Light> lights, Vector3 normal, Vector3 viewDirection, Vector3 fragWorld, 
        LightParameters parameters)
    {
        return ApplyPhongInternal(lights, normal, viewDirection, fragWorld, parameters);
    }

    public static Vector3 ApplyPhongShadingRayTracing(List<Light> lights, Vector3 normal, Vector3 viewDirection, Vector3 fragWorld, LightParameters parameters,
        List<ObjModel> sceneModels)
    {
        return ApplyPhongInternal(lights, normal, viewDirection, fragWorld, parameters, sceneModels);
    }

    public static Color ApplyLambert(List<Light> lambertLights, Vector3 normal, Color baseColor)
    {
        var totalIntensity = 0f;
        foreach (var light in lambertLights)
        {
            var lightDir = Vector3.Normalize(light.Position);

            var intensity = MathF.Max(Vector3.Dot(normal, lightDir), 0);

            totalIntensity += intensity;
        }

        // Ограничиваем суммарную интенсивность значением 1
        totalIntensity = MathF.Min(totalIntensity, 1.0f);

        return System.Windows.Media.Color.FromArgb(
            baseColor.A,
            (byte)(baseColor.R * totalIntensity),
            (byte)(baseColor.G * totalIntensity),
            (byte)(baseColor.B * totalIntensity));
    }
    
    private static (Vector3 diffuse, Vector3 specular) CalculateLightComponents(Vector3 lightDirection, Vector3 normal, Vector3 viewDirection, Light light, 
        LightParameters parameters)
    {
        // Диффузная компонента
        float diff = MathF.Max(Vector3.Dot(normal, lightDirection), 0);
        var diffuse = light.Color * parameters.DiffuseColor * diff * parameters.Kd;

        // Зеркальная компонента
        Vector3 reflectDir = Vector3.Reflect(-lightDirection, normal);
        float spec = MathF.Pow(MathF.Max(Vector3.Dot(viewDirection, reflectDir), 0), parameters.Shininess);
        var specular = light.Color * parameters.SpecularColor * spec * parameters.Ks;

        return (diffuse, specular);
    }
    
    /// <summary>
    /// Проверяет, находится ли фрагмент в тени относительно заданного источника света.
    /// Метод создаёт луч (shadowRay) от точки фрагмента, немного смещённой от поверхности, в направлении источника света.
    /// Затем для каждого объекта сцены проверяется пересечение этого луча с BVH-деревом модели.
    /// Если расстояние до пересечения меньше расстояния до источника света, считается, что фрагмент находится в тени.
    /// </summary>
    /// <param name="fragWorld">Мировые координаты фрагмента (точки поверхности)</param>
    /// <param name="lightPosition">Мировые координаты источника света</param>
    /// <param name="normal">Нормаль к поверхности в точке фрагмента</param>
    /// <param name="sceneModels">Список моделей сцены, для которых построены BVH-деревья</param>
    /// <returns>True, если фрагмент находится в тени, иначе false</returns>
    private static bool IsInShadow(Vector3 fragWorld, Vector3 lightPosition, Vector3 normal, List<ObjModel>? sceneModels)
    {
        if (sceneModels == null) return false;

        // Вычисляем направление от фрагмента к источнику света, нормализованное до единичной длины.
        var lightDirection = Vector3.Normalize(lightPosition - fragWorld);
        
        // Создаем луч для проверки теней.
        // Начало луча немного смещается от поверхности (на величину Triangle.Bias) вдоль нормали,
        // чтобы избежать самопересечений (shadow acne).
        var shadowRay = new Ray
        {
            Origin = fragWorld + normal * Triangle.Bias,
            Direction = lightDirection
        };

        foreach (var model in sceneModels)
        {
            // Если луч пересекает BVH-дерево модели,
            // и расстояние до пересечения меньше расстояния до источника света,
            // значит, перед фрагментом находится какой-либо объект, который отбрасывает тень.
            if (ShadowHelper.RayIntersectBvh(model.BvhTree!, shadowRay, out var t) 
                && t < Vector3.Distance(fragWorld, lightPosition))
            {
                return true;
            }
        }
        
        // Если для ни одной модели не найдено препятствие на пути к источнику света,
        // фрагмент не находится в тени
        return false;
    }
    
    private static Vector3 ApplyPhongInternal(List<Light> lights, Vector3 normal, Vector3 viewDirection,
        Vector3 fragWorld, LightParameters parameters, List<ObjModel>? sceneModels = null)
    {
        var ambient = parameters.AmbientColor * parameters.Ka;
        var lighting = ambient;

        foreach (var light in lights)
        {
            var lightDirection = Vector3.Normalize(light.Position - fragWorld);
            
            if (!IsInShadow(fragWorld, light.Position, normal, sceneModels))
            {
                var (diffuse, specular) = CalculateLightComponents(lightDirection, normal, viewDirection,
                    light, parameters);

                lighting += (diffuse + specular) * light.Intensity;
            }
        }

        return Vector3.Clamp(lighting, Vector3.Zero, new Vector3(255, 255, 255));
    }

    public Vector3 TransformLightToScreen(Matrix4x4 view, Matrix4x4 projection, Matrix4x4 viewport)
    {
        var transformedPosition = Vector4.Transform(Position, view * projection * viewport);

        if (transformedPosition.W != 0)
        {
            transformedPosition /= transformedPosition.W;
        }

        return transformedPosition.AsVector3();
    }
}