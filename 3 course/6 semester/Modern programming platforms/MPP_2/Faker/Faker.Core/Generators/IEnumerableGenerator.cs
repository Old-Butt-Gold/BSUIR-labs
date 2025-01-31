using System.Collections;
using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class IEnumerableGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First();
        
        var enumerable = (IList)context.Faker.Create(typeof(List<>)
            .MakeGenericType(type));
        return enumerable;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType 
               && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }
}