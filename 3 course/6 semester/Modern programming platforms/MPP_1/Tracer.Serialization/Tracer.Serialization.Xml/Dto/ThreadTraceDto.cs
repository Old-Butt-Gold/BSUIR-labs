using System.Xml.Serialization;
using Tracer.Serialization.Xml.Dto;

namespace Tracer.Serialization.Xml.Dto;

public class ThreadTraceDto
{
    [XmlAttribute("id")]
    public int ThreadId { get; set; }

    [XmlAttribute("time")]
    public string TotalExecutionTime { get; set; }

    [XmlArray("methods")]
    [XmlArrayItem("method")]
    public List<MethodTraceDto> Methods { get; set; } = new();
}