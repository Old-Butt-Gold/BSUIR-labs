namespace Faker.Core.Interfaces;

// Самый общий интерфейс, с коллекцией объектов которого можно работать,
// и вызывать, не используя Reflection.
public interface IValueGenerator
{
    object Generate(Type typeToGenerate, GeneratorContext context);
    // Позволяет реализовывать сколь угодно сложную логику определения,
    // подходит ли генератор. Таким образом можно работать с генераторами
    // коллекций аналогично генераторам примитивных типов.
    bool CanGenerate(Type type);
}