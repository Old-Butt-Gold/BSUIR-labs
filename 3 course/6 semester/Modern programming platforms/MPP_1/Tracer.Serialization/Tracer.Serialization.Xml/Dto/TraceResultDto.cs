using System.Xml.Serialization;

namespace Tracer.Serialization.Xml.Dto;

[XmlRoot("traceResult")]
public class TraceResultDto
{
    [XmlArray("threads")]
    [XmlArrayItem("thread")]
    public List<ThreadTraceDto> Threads { get; set; } = new();
}