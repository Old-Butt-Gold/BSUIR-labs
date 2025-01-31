using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class DateTimeGenerator : IValueGenerator
{
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        DateTime start = new DateTime(1950, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(context.Random.Next(range));
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(DateTime);
    }
}