using System.Numerics;
using System.Windows.Media;
using AKG.Core.Extensions;

namespace AKG.Core.Objects;

public struct LightParameters
{
    public static LightParameters DefaultLightParameters { get; } = new();
    
    public LightParameters()
    { }

    public LightParameters(Material material)
    {
        Ka = material.Ka;
        AmbientColor = material.AmbientColor;
        DiffuseColor = material.DiffuseColor;
        Kd = material.Kd;
        SpecularColor = material.SpecularColor;
        Ks = material.Ks;
        Shininess = material.Shininess;
    }

    public LightParameters(Vector3 ambientColor, Vector3 ka,
        Vector3 diffuseColor, Vector3 kd, Vector3 specularColor, Vector3 ks, float shininess)
    {
        Ka = ka;
        AmbientColor = ambientColor;
        DiffuseColor = diffuseColor;
        Kd = kd;
        SpecularColor = specularColor;
        Ks = ks;
        Shininess = shininess;
    }
    
    /// <summary>
    ///     Коэффициент фонового (амбиентного) освещения
    /// </summary>
    public Vector3 Ka { get; set; } = new(0.1f);

    /// <summary>
    ///     Коэффициент рассеянного (диффузного) освещения
    /// </summary>
    public Vector3 Kd { get; set; } = new(1.0f);

    /// <summary>
    ///     Коэффициент зеркального освещения
    /// </summary>
    public Vector3 Ks { get; set; } = new(0.2f);

    /// <summary>
    ///     Эмиссивная компонента
    /// </summary>
    public Vector3 Ke { get; set; } = new(1.0f);

    /// <summary>
    ///     Показатель блеска поверхности. (NS)
    /// </summary>
    public float Shininess { get; set; } = 64f;

    // Эти должны изменяться для каждого пикселя грани (face)

    /// <summary>
    ///     Амбиентная компонента освещения.
    /// </summary>
    public Vector3 AmbientColor { get; set; } = Colors.Black.ToVector3();

    /// <summary>
    ///     Диффузная компонента освещения.
    /// </summary>
    public Vector3 DiffuseColor { get; set; } = Colors.Gray.ToVector3();

    /// <summary>
    ///     Зеркальная компонента освещения.
    /// </summary>
    public Vector3 SpecularColor { get; set; } = Colors.White.ToVector3();
}