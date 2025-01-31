using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class DoubleGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        return context.Random.NextDouble() * double.MaxValue;
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(double);
    }
}