using System.Globalization;
using System.IO;
using System.Numerics;

namespace AKG.Core.Parser;

public static class ObjParser
{
    /// <summary>
    /// Парсит файл .obj и возвращает объект модели.
    /// При этом рассчитывается ограничивающий прямоугольный параллелепипед
    /// bounding box (min, max) для всех вершин, которые полностью охватывают объект;
    /// коэффициент масштабирования (Scale) и дополнительная величина (Delta).
    /// </summary>
    /// <param name="filePath">Путь к файлу .obj</param>
    /// <returns>Объект ObjModel, содержащий прочитанные данные</returns>
    public static ObjModel Parse(string filePath)
    {
        var model = new ObjModel();

        // Для преобразования чисел с точкой
        var culture = CultureInfo.InvariantCulture;
        
        Vector4 min = new Vector4(float.MaxValue, float.MaxValue, float.MaxValue, 1.0f);
        Vector4 max = new Vector4(float.MinValue, float.MinValue, float.MinValue, 1.0f);

        int lineIndex = 0;

        foreach (var line in File.ReadLines(filePath))
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#"))
                continue;

            var tokens = trimmedLine.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0)
                continue;

            switch (tokens[0])
            {
                case "v": // Геометрическая вершина
                {
                    // Ожидается: v x y z [w]
                    if (tokens.Length < 4)
                    {
                        throw new ArgumentException($"Неправильный формат вершины на {lineIndex} строке");
                    }

                    float x = float.Parse(tokens[1], culture);
                    float y = float.Parse(tokens[2], culture);
                    float z = float.Parse(tokens[3], culture);
                    float w = tokens.Length >= 5 ? float.Parse(tokens[4], culture) : 1.0f;
                    Vector4 vertex = new Vector4(x, y, z, w);
                    model.OriginalVertices.Add(vertex);

                    // Обновляем bounding box (min/max) по осям X, Y и Z
                    if (vertex.X < min.X) min.X = vertex.X;
                    if (vertex.Y < min.Y) min.Y = vertex.Y;
                    if (vertex.Z < min.Z) min.Z = vertex.Z;

                    if (vertex.X > max.X) max.X = vertex.X;
                    if (vertex.Y > max.Y) max.Y = vertex.Y;
                    if (vertex.Z > max.Z) max.Z = vertex.Z;
                    break;
                }
                case "vt": // Текстурная координата
                {
                    // Ожидается: vt u [v] [w]
                    if (tokens.Length < 2)
                    {
                        throw new ArgumentException($"Неверный формат текстурной координаты на ${lineIndex} строке");
                    }

                    float u = float.Parse(tokens[1], culture);
                    float v = tokens.Length >= 3 ? float.Parse(tokens[2], culture) : 0;
                    float w = tokens.Length >= 4 ? float.Parse(tokens[3], culture) : 0;
                    model.TextureCoords.Add(new(u, v, w));
                    break;
                }
                case "vn": // Нормаль вершины
                {
                    // Ожидается: vn i j k
                    if (tokens.Length < 4)
                    {
                        throw new ArgumentException($"Неверный формат нормали на ${lineIndex} строке");
                    }

                    float i = float.Parse(tokens[1], culture);
                    float j = float.Parse(tokens[2], culture);
                    float k = float.Parse(tokens[3], culture);
                    model.Normals.Add(new (i, j, k));
                    break;
                }
                case "f": // Грань (полигон)
                {
                    // Ожидается: f v1 v2 v3 ... (каждый v может быть v, v/vt, v//vn или v/vt/vn)
                    if (tokens.Length < 4)
                    {
                        throw new ArgumentException($"Неверный формат грани (требуется минимум 3 вершины) на ${lineIndex} строке");
                    }

                    var face = new Face();

                    for (int i = 1; i < tokens.Length; i++)
                    {
                        var faceVertex = new FaceVertex();
                        
                        if (tokens.Contains("//"))
                        {
                            var parts = tokens[i].Split("//");

                            if (int.TryParse(parts[0], out var vertexIndex))
                            {
                                faceVertex.VertexIndex = vertexIndex;
                            }
                            else
                            {
                                throw new ArgumentException($"Ошибка парсинга индекса вершины на ${lineIndex} строке");
                            }

                            if (parts.Length > 1 && int.TryParse(parts[1], out var normIndex))
                            {
                                faceVertex.NormalIndex = normIndex;
                            }
                            else
                            {
                                throw new ArgumentException($"Ошибка парсинга индекса нормали на ${lineIndex} строке");
                            }
                        }
                        else
                        {
                            var parts = tokens[i].Split('/');

                            // Индекс вершины (обязательный)
                            if (int.TryParse(parts[0], out var vertexIndex))
                            {
                                faceVertex.VertexIndex = vertexIndex;
                            }
                            else
                            {
                                throw new ArgumentException($"Ошибка парсинга индекса вершины на ${lineIndex} строке");
                            }

                            // Если присутствует текстурный индекс
                            if (parts.Length > 1 && !string.IsNullOrEmpty(parts[1]))
                            {
                                if (int.TryParse(parts[1], out var texIndex))
                                {
                                    faceVertex.TextureIndex = texIndex;
                                }
                                else
                                {
                                    throw new ArgumentException(
                                        $"Ошибка парсинга текстурного индекса на ${lineIndex} строке");
                                }
                            }

                            // Если присутствует нормаль
                            if (parts.Length > 2 && !string.IsNullOrEmpty(parts[2]))
                            {
                                if (int.TryParse(parts[2], out var normIndex))
                                {
                                    faceVertex.NormalIndex = normIndex;
                                }
                                else
                                {
                                    throw new ArgumentException(
                                        $"Ошибка парсинга индекса нормали на ${lineIndex} строке");
                                }
                            }
                        }
                        
                        face.Vertices.Add(faceVertex);
                    }

                    model.Faces.Add(face);
                    break;
                }
                default:
                    break;
            }

            lineIndex++;
        }

        var diff = Vector4.Abs(max - min);
        
        // Определяем максимальный размер по осям
        float maxDiff = MathF.Max(diff.X, MathF.Max(diff.Y, diff.Z));
        float scale = 2.0f / (maxDiff == 0 ? 1 : maxDiff);
        float delta = scale / 10.0f; // к примеру шаг изменения 10

        model.Min = min;
        model.Max = max;
        model.Delta = delta;
        model.TransformedVertices = new Vector4[model.OriginalVertices.Count];
        
        return model;
    }
}