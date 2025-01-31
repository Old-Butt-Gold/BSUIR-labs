using System.Linq.Expressions;
using System.Reflection;
using Faker.Core.Interfaces;

namespace Faker.Core;

public class FakerConfig
{
    private readonly Dictionary<(Type ObjectType, MemberInfo Member), IValueGenerator> _customGenerators = new();

    public void Add<T, TMember, TGenerator>(Expression<Func<T, TMember>> expression)
        where TGenerator : IValueGenerator, new()
    {
        if (expression.Body is not MemberExpression memberExpression)
            throw new ArgumentException("Invalid expression. Expected member access to a field or property.", nameof(expression));

        var member = memberExpression.Member;
        
        if (member is not PropertyInfo && member is not FieldInfo)
            throw new ArgumentException("Member must be a property or field.", nameof(expression));

        _customGenerators[(typeof(T), member)] = new TGenerator();
    }

    public IValueGenerator? GetGenerator(Type objectType, MemberInfo member)
    {
        return _customGenerators.GetValueOrDefault((objectType, member));
    }
}