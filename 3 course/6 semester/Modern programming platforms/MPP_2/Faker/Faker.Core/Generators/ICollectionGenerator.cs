using System.Collections;
using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class ICollectionGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First();
        
        var collection = (ICollection)context.Faker.Create(typeof(List<>)
            .MakeGenericType(type));

        return collection;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType 
               && type.GetGenericTypeDefinition() == typeof(ICollection<>);
    }
}