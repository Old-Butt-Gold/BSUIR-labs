using System.Collections;
using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class IListGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var type = typeToGenerate.GetGenericArguments().First();
        
        var ilist = (IList)context.Faker.Create(typeof(List<>)
            .MakeGenericType(type));
        return ilist;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>);
    }
}