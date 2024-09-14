using System.IO;

namespace SharedComponents.AdapterPluginFunctionality;

public interface ISerializerOption
{
    public object PreprocessorSave(object list);
    public Stream PostprocessorSave(Stream stream);
    public object PreprocessorLoad(object list);
    public Stream PostprocessorLoad(Stream stream);
}