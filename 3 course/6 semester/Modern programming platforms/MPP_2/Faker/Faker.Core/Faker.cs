using System.Reflection;
using Faker.Core.Generators;
using Faker.Core.Interfaces;

namespace Faker.Core;

public class Faker
{
    private readonly List<IValueGenerator> _generators =
    [
        new IntGenerator(),
        new LongGenerator(),
        new DoubleGenerator(),
        new FloatGenerator(),
        new StringGenerator(),
        new DateTimeGenerator(),
        new ListGenerator(),
        new ArrayGenerator(),
        new IEnumerableGenerator(),
        new ICollectionGenerator(),
        new IListGenerator()
    ];
    
    private readonly Random _random = new();
    
    private readonly ThreadLocal<HashSet<Type>> _typesInProcess;

    private readonly FakerConfig? _config;
    
    public Faker(FakerConfig? config = null)
    {
        _typesInProcess = new ThreadLocal<HashSet<Type>>(() => new());
        _config = config;
    }

    public T Create<T>()
    {
        return (T)Create(typeof(T));
    }

    // Может быть вызван изнутри Faker, из IValueGenerator (см. ниже) или пользователем
    public object Create(Type type)
    {
        if (!_typesInProcess.Value!.Add(type))
        {
            return GetDefaultValue(type);
        }

        try
        {
            var generator = _generators.FirstOrDefault(g => g.CanGenerate(type));
            if (generator != null)
                return generator.Generate(type, new(_random, this));

            return CreateComplexObject(type);
        }
        finally
        {
            _typesInProcess.Value.Remove(type);
        }
    }

    private object CreateComplexObject(Type type)
    {
        var constructors = type.GetConstructors(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
            .OrderByDescending(c => c.GetParameters().Length)
            .ToList();

        foreach (var constructor in constructors)
        {
            try
            {
                var parameters = constructor.GetParameters();
                var parameterValues = parameters
                    .Select(p => GenerateParameterValue(type, p)).ToArray();
                var instance = constructor.Invoke(parameterValues);
                InitializePropertiesAndFields(instance, parameters);
                return instance;
            }
            catch
            {
                // Ignore and try next constructor
            }
        }
        
        return GetDefaultValue(type);
    }
    
    private object GenerateParameterValue(Type objectType, ParameterInfo parameter)
    {
        // Сначала пытаемся найти член по имени параметра
        var memberByName = FindMemberForParameter(objectType, parameter);
        if (memberByName != null)
        {
            var generator = _config?.GetGenerator(objectType, memberByName);
            if (generator != null && generator.CanGenerate(parameter.ParameterType))
                return generator.Generate(parameter.ParameterType, new (_random, this));
        }
        
        // Если не нашли по имени, ищем любой член с таким же типом
        var membersByType = objectType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m is PropertyInfo or FieldInfo)
            .Where(m => GetMemberType(m) == parameter.ParameterType);

        foreach (var member in membersByType)
        {
            var generator = _config?.GetGenerator(objectType, member);
            if (generator != null && generator.CanGenerate(parameter.ParameterType))
                return generator.Generate(parameter.ParameterType, new GeneratorContext(_random, this));
        }
        
        // Если ничего не нашли, создаём стандартным образом
        return Create(parameter.ParameterType);
        
        static MemberInfo? FindMemberForParameter(Type objectType, ParameterInfo parameter)
        {
            var paramName = parameter.Name;
            var paramType = parameter.ParameterType;

            return objectType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m is PropertyInfo or FieldInfo)
                .FirstOrDefault(m =>
                    string.Equals(m.Name, paramName, StringComparison.OrdinalIgnoreCase) &&
                    GetMemberType(m) == paramType);
        
            
        }
        
        static Type GetMemberType(MemberInfo member) =>
            member switch
            {
                PropertyInfo p => p.PropertyType,
                FieldInfo f => f.FieldType,
                _ => throw new InvalidOperationException()
            };
    }
    
    
    
    private void InitializePropertiesAndFields(object instance, ParameterInfo[] constructorParameters)
    {
        var type = instance.GetType();
        var parameterNames = new HashSet<string>(
            constructorParameters.Select(p => p.Name)!,
            StringComparer.OrdinalIgnoreCase);
        
        foreach (var prop in type
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.CanWrite 
                                 && p.SetMethod!.IsPublic 
                                 && !parameterNames.Contains(p.Name)))
        {
            var generator = _config?.GetGenerator(type, prop);
            var value = generator != null && generator.CanGenerate(prop.PropertyType)
                ? generator.Generate(prop.PropertyType, new (_random, this))
                : Create(prop.PropertyType);
            prop.SetValue(instance, value);
        }

        foreach (var field in
                 type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                     .Where(f => !f.IsInitOnly && !parameterNames.Contains(f.Name)))
        {
            var generator = _config?.GetGenerator(type, field);
            var value = generator != null && generator.CanGenerate(field.FieldType)
                ? generator.Generate(field.FieldType, new(_random, this))
                : Create(field.FieldType);
            field.SetValue(instance, value);
        }
    }
    
    private static object GetDefaultValue(Type t)
    {
        // Для типов-значений вызов конструктора по умолчанию даст default(T)
        // Для ссылочных типов значение по умолчанию всегда null.
        return (t.IsValueType ? Activator.CreateInstance(t) : null)!;
    }

}