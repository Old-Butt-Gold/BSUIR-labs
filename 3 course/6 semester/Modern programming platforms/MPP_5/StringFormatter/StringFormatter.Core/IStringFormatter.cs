namespace StringFormatter.Core;

public interface IStringFormatter
{
    string Format(string template, object target);
}