using Faker.Core.Interfaces;

namespace Faker.Core.Generators;

public class StringGenerator : IValueGenerator
{
    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    
    public object Generate(Type typeToGenerate, GeneratorContext context)
    {
        int length = context.Random.Next(5, 20);
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[context.Random.Next(s.Length)]).ToArray());
    }

    public bool CanGenerate(Type type)
    {
        return type == typeof(string);
    }
}