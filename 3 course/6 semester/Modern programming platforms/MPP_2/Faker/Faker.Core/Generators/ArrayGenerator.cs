using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class ArrayGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        Type elementType = typeToGenerate.GetElementType()!;
        int length = context.Random.Next(2, 10);
        Array array = Array.CreateInstance(elementType, length);

        for (int i = 0; i < length; i++)
        {
            array.SetValue(context.Faker.Create(elementType), i);
        }

        return array;
    }

    public bool CanGenerate(Type type)
    {
        return type.IsArray;
    }
}