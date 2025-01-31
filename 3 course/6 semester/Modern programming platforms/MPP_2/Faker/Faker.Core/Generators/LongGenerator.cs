using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class LongGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextInt64();
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(long);
    }
}