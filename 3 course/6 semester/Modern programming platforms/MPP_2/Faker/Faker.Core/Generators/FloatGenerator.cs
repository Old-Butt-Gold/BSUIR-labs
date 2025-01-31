using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class FloatGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextSingle() * float.MaxValue;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(float);
    }
}