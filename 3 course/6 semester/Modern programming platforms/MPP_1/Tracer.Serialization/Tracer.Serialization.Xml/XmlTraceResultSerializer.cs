using System.Xml.Serialization;
using Tracer.Core;
using Tracer.Serialization.Abstractions;
using Tracer.Serialization.Xml.Dto;

namespace Tracer.Serialization.Xml;

public class XmlTraceResultSerializer : ITraceResultSerializer
{
    public string Format => "xml";

    public void Serialize(TraceResult traceResult, Stream to)
    {
        var dto = TraceResultConverter.ToDto(traceResult);
        var serializer = new XmlSerializer(typeof(TraceResultDto));
        serializer.Serialize(to, dto);
    }
}