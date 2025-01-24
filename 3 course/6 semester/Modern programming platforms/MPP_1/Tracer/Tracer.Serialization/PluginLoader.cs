using System.Reflection;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization;

public static class PluginLoader
{
    public static IList<ITraceResultSerializer> LoadSerializers()
    {
        var serializers = new List<ITraceResultSerializer>();
        var pluginDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

        if (!Directory.Exists(pluginDirectory))
        {
            Console.WriteLine("Plugins directory not found.");
            return serializers;
        }
        
        var assemblies = Directory.GetFiles(pluginDirectory, "*.dll");

        foreach (var assemblyPath in assemblies)
        {
            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                
                var types = assembly.GetTypes()
                    .Where(t => typeof(ITraceResultSerializer)
                        .IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

                foreach (var type in types)
                {
                    if (Activator.CreateInstance(type) is ITraceResultSerializer serializer)
                    {
                        serializers.Add(serializer);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
        
        return serializers;
    }
}