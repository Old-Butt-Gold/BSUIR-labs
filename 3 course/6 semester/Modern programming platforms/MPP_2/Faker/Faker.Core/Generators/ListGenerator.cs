using System.Collections;
using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class ListGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        var elementType = typeToGenerate.GetGenericArguments().FirstOrDefault();
        if (elementType == null) return null!;
        
        var list = (IList)Activator.CreateInstance(typeof(List<>)
            .MakeGenericType(elementType))!;

        int count = context.Random.Next(2, 10);
        for (int i = 0; i < count; i++)
        {
            list.Add(context.Faker.Create(elementType));
        }

        return list;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsGenericType
               && type.GetGenericTypeDefinition() == typeof(List<>);
    }
}