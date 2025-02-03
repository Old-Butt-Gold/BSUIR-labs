using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace StringFormatter.Core;

public class StringFormatter : IStringFormatter
{
    public static readonly StringFormatter Shared = new();
    private static readonly ConcurrentDictionary<string, Delegate> Cache = [];

    public string Format(string template, object target)
    {
        ArgumentNullException.ThrowIfNull(target, nameof(target));
        ArgumentNullException.ThrowIfNull(template, nameof(template));

        var type = target.GetType();
        var sb = new StringBuilder();
        var propertyBuffer = new StringBuilder();
        bool insideBraces = false;
        
        for (int i = 0; i < template.Length; i++)
        {
            char c = template[i];

            if (insideBraces)
            {
                if (c == '}')
                {
                    insideBraces = false;
                    string propertyName = propertyBuffer.ToString();
                    sb.Append(GetPropertyValue(target, type, propertyName));
                    propertyBuffer.Clear();
                }
                else
                {
                    propertyBuffer.Append(c);
                }
            }
            else
            {
                switch (c)
                {
                    // Check for escaped '{'
                    case '{' when i + 1 < template.Length && template[i + 1] == '{':
                        sb.Append('{');
                        i++; // Skip next character
                        break;
                    case '{':
                        insideBraces = true;
                        propertyBuffer.Clear();
                        break;
                    // Check for escaped '}'
                    case '}' when i + 1 < template.Length && template[i + 1] == '}':
                        sb.Append('}');
                        i++; // Skip next character
                        break;
                    case '}':
                        throw new FormatException("Unbalanced curly braces in format string.");
                    default:
                        sb.Append(c);
                        break;
                }
            }
        }

        if (insideBraces)
            throw new FormatException("Unbalanced curly braces in format string.");

        return sb.ToString();
    }

    private static string GetPropertyValue(object target, Type type, string propertyName)
    {
        string cacheKey = $"{type.FullName}_{propertyName}";
        if (!Cache.TryGetValue(cacheKey, out var accessor))
        {
            accessor = CreateAccessor(type, propertyName);
            Cache[cacheKey] = accessor;
        }

        return accessor.DynamicInvoke(target)?.ToString() ?? string.Empty;
        
        static Delegate CreateAccessor(Type type, string propertyName)
        {
            string[] parts = propertyName.Split(['[', ']', '.'], StringSplitOptions.RemoveEmptyEntries);
            ParameterExpression param = Expression.Parameter(type, "target");
            
            Expression body = param;
            var currentType = type;

            foreach (string part in parts)
            {
                if (int.TryParse(part, out int index))
                {
                    if (currentType is { IsArray: false, IsGenericType: false })
                        throw new FormatException($"Invalid index format in property '{propertyName}'.");

                    body = Expression.ArrayIndex(body, Expression.Constant(index));
                    currentType = currentType.GetElementType() ?? currentType.GenericTypeArguments[0];
                }
                else
                {
                    var property = currentType.GetProperty(part)
                                   ?? throw new FormatException($"Property '{part}' not found in type '{currentType.Name}'.");

                    body = Expression.Property(body, property);
                    currentType = property.PropertyType;
                }
            }

            var lambda = Expression.Lambda(body, param);
            return lambda.Compile();
        }
    }
}