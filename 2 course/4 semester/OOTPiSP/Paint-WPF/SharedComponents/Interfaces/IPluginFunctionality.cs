using SharedComponents.AbstractClasses;

namespace SharedComponents.Interfaces;

public interface IPluginFunctionality
{
    public string Name { get; }
    public void SaveToFile(List<AbstractShape> abstractShapes);
    public (bool result, List<AbstractShape>? abstractShapes) LoadFile(Dictionary<object, AbstractFactory> dictionary);
}