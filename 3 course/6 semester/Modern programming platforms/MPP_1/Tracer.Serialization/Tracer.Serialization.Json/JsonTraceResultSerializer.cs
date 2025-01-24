using Tracer.Core;
using Tracer.Serialization.Abstractions;

namespace Tracer.Serialization.Json;

public class JsonTraceResultSerializer : ITraceResultSerializer
{
    public string Format => "json";

    public void Serialize(TraceResult traceResult, Stream to)
    {
        using var writer = new StreamWriter(to);
        var json = System.Text.Json.JsonSerializer.Serialize(traceResult, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        writer.Write(json);
    }
}