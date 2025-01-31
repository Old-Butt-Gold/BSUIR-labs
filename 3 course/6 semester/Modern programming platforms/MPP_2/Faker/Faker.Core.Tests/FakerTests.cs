using System.Collections.ObjectModel;
using Faker.Core.Interfaces;

namespace Faker.Core.Tests;

public class FakerTests
{
    private readonly Faker _faker = new();
    
    [Fact]
    // Тестируемый метод/функциональность
    // Сценарий тестирования
    // Ожидаемый результат
    public void Create_UserType_ReturnsInitializedObject()
    {
        // AAA – arrange, act, assert
        var user = _faker.Create<User>();

        Assert.NotNull(user);
        Assert.NotNull(user.Name);
        Assert.InRange(user.Age, 1, int.MaxValue);
    }

    [Fact]
    public void Create_GenericList_ReturnsPopulatedList()
    {
        var users = _faker.Create<List<User>>();

        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.All(users, u => Assert.NotNull(u.Name));
    }

    [Fact] 
    public void Create_TypeWithCyclicDependency_BreaksCycleByNull()
    {
        var a = _faker.Create<A>();

        Assert.NotNull(a.B);
        Assert.NotNull(a.B.C);
        Assert.Null(a.B.C.A);
    }
    
    [Fact]
    public void Create_PrimitiveTypeInt_ReturnsNonDefaultValue()
    {
        var result = _faker.Create<int>();
        Assert.NotEqual(default, result);
    }

    [Fact]
    public void Create_DateTimeType_ReturnsValidDateInRange()
    {
        var result = _faker.Create<DateTime>();
        Assert.InRange(result, DateTime.MinValue, DateTime.MaxValue);
    }

    [Fact]
    public void Create_LongType_ReturnsValidLong()
    {
        var result = _faker.Create<long>();
        Assert.InRange(result, long.MinValue, long.MaxValue);
    }

    [Fact]
    public void Create_DoubleType_ReturnsValidDouble()
    {
        var result = _faker.Create<double>();
        Assert.InRange(result, double.MinValue, double.MaxValue);
    }

    [Fact]
    public void Create_FloatType_ReturnsValidFloat()
    {
        var result = _faker.Create<float>();
        Assert.InRange(result, float.MinValue, float.MaxValue);
    }

    [Fact]
    public void Create_StringType_ReturnsNonEmptyString()
    {
        var result = _faker.Create<string>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public void Create_ArrayType_ReturnsNonEmptyArray()
    {
        var array = _faker.Create<int[]>();
        
        Assert.NotNull(array);
        Assert.NotEmpty(array);
        Assert.All(array, x => Assert.NotEqual(default, x));
    }

    [Fact]
    public void Create_ReadOnlyCollection_ReturnsInitializedCollection()
    {
        var collection = _faker.Create<ReadOnlyCollection<string>>();
        
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
    }

    [Fact]
    public void Create_IEnumerable_ReturnsCollection()
    {
        var collection = _faker.Create<IEnumerable<string>>();
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
    }

    [Fact]
    public void Create_ICollection_ReturnsMutableCollection()
    {
        var collection = _faker.Create<ICollection<DateTime>>();
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
        collection.Add(DateTime.Now); 
    }

    [Fact]
    public void Create_IList_ReturnsListImplementation()
    {
        var list = _faker.Create<IList<double>>();
        Assert.NotNull(list);
        Assert.NotEmpty(list);
        Assert.IsAssignableFrom<IList<double>>(list);
    }
    
    [Fact]
    public void Create_Dictionary_ShouldBeNull()
    {
        var dictionary = _faker.Create<Dictionary<string, int>>();
        Assert.Null(dictionary);
    }

    [Fact]
    public void Create_StructWithConstructor_InitializesCorrectly()
    {
        var result = _faker.Create<TestStruct>();
        Assert.NotEqual(default(TestStruct), result);
        Assert.True(result.Value > 0);
    }

    [Fact]
    public void Create_PrivateConstructorType_ReturnsInstance()
    {
        var result = _faker.Create<SingletonType>();
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Value);
    }
    
    [Fact]
    public void CustomGenerator_ShouldAssignToMatchingConstructorParameter_WhenNamesDiffer()
    {
        var config = new FakerConfig();
        config.Add<Person, string, CityGenerator>(p => p.City);
        var faker = new Faker(config);

        var person = faker.Create<Person>();

        Assert.Equal("Minsk", person.City);
    }
    
    [Fact]
    public void CustomGenerator_ShouldAssignToMatchingField_WhenThereIsNoConstructors()
    {
        var config = new FakerConfig();
        config.Add<PersonField, string, CityGenerator>(p => p.Name);
        var faker = new Faker(config);

        var person = faker.Create<PersonField>();

        Assert.Equal("Minsk", person.Name);
    }

    [Fact]
    public void MultipleCustomGenerators_ShouldAssignToCorrectConstructorParameters_ByTypeMatching()
    {
        var config = new FakerConfig();
        config.Add<Person, string, CityGenerator>(p => p.City);
        config.Add<Person, int, AgeGenerator>(p => p.Age);
        var faker = new Faker(config);

        var person = faker.Create<Person>();

        Assert.Equal("Minsk", person.City);
        Assert.Equal(42, person.Age);
    }

    [Fact]
    public void CustomGenerator_ShouldInitializeReadOnlyProperty_ThroughConstructor()
    {
        var config = new FakerConfig();
        config.Add<ImmutableUser, string, NameGenerator>(u => u.Name);
        var faker = new Faker(config);

        var user = faker.Create<ImmutableUser>();

        Assert.Equal("GeneratedName", user.Name);
    }
    
    [Fact]
    public void CyclicDependency_ShouldBreakCycle_WhenUsingCustomGenerators()
    {
        var config = new FakerConfig();
        config.Add<A, B, BGenerator>(a => a.B);
        var faker = new Faker(config);

        var a = faker.Create<A>();

        Assert.NotNull(a.B);
        Assert.Null(a.B.C); 
    }
}

public class A
{
    public B B { get; set; }
}

public class B
{
    public C C { get; set; }
}

public class C
{
    public A A { get; set; }
}

class User
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public struct TestStruct
{
    public int Value { get; }

    public TestStruct(int value)
    {
        Value = value;
    }
}

public class SingletonType
{
    public int Value { get; }

    private SingletonType(int value)
    {
        Value = value;
    }
}

public class Person
{
    public string City { get; }
    public int Age { get; }

    public Person(string cityParam, int ageParameter)
    {
        City = cityParam;
        Age = ageParameter;
    }
}

public class PersonField
{
    public string Name;
}

public class ImmutableUser
{
    public string Name { get; }

    public ImmutableUser(string nameParam)
    {
        Name = nameParam;
    }
}

public class CityGenerator : IValueGenerator
{
    public object Generate(Type type, GeneratorContext context) => "Minsk";
    public bool CanGenerate(Type type) => type == typeof(string);
}

public class AgeGenerator : IValueGenerator
{
    public object Generate(Type type, GeneratorContext context) => 42;
    public bool CanGenerate(Type type) => type == typeof(int);
}

public class NameGenerator : IValueGenerator
{
    public object Generate(Type type, GeneratorContext context) => "GeneratedName";
    public bool CanGenerate(Type type) => type == typeof(string);
}

public class BGenerator : IValueGenerator
{
    public object Generate(Type type, GeneratorContext context) => new B();
    public bool CanGenerate(Type type) => type == typeof(B);
}