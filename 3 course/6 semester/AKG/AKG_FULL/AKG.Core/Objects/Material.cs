using System.Numerics;
using System.Windows.Media;
using AKG.Core.Extensions;

namespace AKG.Core.Objects;

/// <summary>
///     Класс, описывающий свойства материала.
/// </summary>
public class Material
{
    public static Material DefaultMaterial { get; } = new();

    /// <summary>
    ///     Имя материала.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Путь к текстуре диффузного цвета (map_Kd).
    /// </summary>
    public string DiffuseMap { get; set; } = string.Empty;

    /// <summary>
    ///     Путь к эмиссивной текстуре (map_Ke), если есть.
    /// </summary>
    public string EmissiveMap { get; set; } = string.Empty;

    /// <summary>
    ///     Путь к нормальной карте (norm).
    /// </summary>
    public string NormalMap { get; set; } = string.Empty;

    /// <summary>
    ///     Путь к текстуре карты MRAO (map_MRAO) (metallic-roughness-ambient occlusion).
    /// </summary>
    public string MraoMap { get; set; } = string.Empty;

    /// <summary>
    ///     Путь к текстуре карты Specular Map
    /// </summary>
    public string SpecularMap { get; set; } = string.Empty;

    // Коэффициенты (берутся из .mtl)

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