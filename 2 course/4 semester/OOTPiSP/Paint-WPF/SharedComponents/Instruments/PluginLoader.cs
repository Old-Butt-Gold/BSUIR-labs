using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Win32;
using SharedComponents.AbstractClasses;
using SharedComponents.Interfaces;

namespace SharedComponents.Instruments;

public class PluginLoader
{
    public List<AbstractFactory> LoadPluginShape()
    {
        List<AbstractFactory> list = [];
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Динамическая библиотека (*.dll)|*.dll"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            var context = new AssemblyLoadContext("DynamicLoad", true);
            Assembly assembly = context.LoadFromAssemblyPath(openFileDialog.FileName);
            var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(AbstractFactory)));
            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is AbstractFactory factory)
                {
                    list.Add(factory);
                }
            }
            context.Unload();
        }

        return list;
    }
    
    public List<IPluginFunctionality> LoadPluginFunctionality()
    {
        List<IPluginFunctionality> list = [];
        OpenFileDialog openFileDialog = new()
        {
            Filter = "Динамическая библиотека (*.dll)|*.dll"
        };
        if (openFileDialog.ShowDialog() == true)
        {
            var context = new AssemblyLoadContext("DynamicLoad", true);
            Assembly assembly = context.LoadFromAssemblyPath(openFileDialog.FileName);
            var type = assembly.GetTypes().FirstOrDefault(type => typeof(IPluginFunctionality).IsAssignableFrom(type));
            if (type != null && Activator.CreateInstance(type) is IPluginFunctionality pluginFunctionality)
            {
                list.Add(pluginFunctionality);
            }

            context.Unload();
        }

        return list;
    }

    public List<AbstractFactory> LoadPluginInCurrentAssembly()
    {
        List<AbstractFactory> list = [];
        //Assembly assembly = Assembly.GetExecutingAssembly(); Должно тогда лежать в папке OOTPISP
        Assembly assembly = Assembly.GetEntryAssembly()!;
        var types = assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(AbstractFactory)));
        foreach (var type in types)
        {
            if (Activator.CreateInstance(type) is AbstractFactory factory)
            {
                list.Add(factory);
            }
        }

        return list;
    }
}