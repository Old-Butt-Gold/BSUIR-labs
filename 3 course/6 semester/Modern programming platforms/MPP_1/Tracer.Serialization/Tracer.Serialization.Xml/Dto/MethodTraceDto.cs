using System.Xml.Serialization;

namespace Tracer.Serialization.Xml.Dto;

public class MethodTraceDto
{
    [XmlElement("name")]
    public string MethodName { get; set; }

    [XmlElement("class")]
    public string ClassName { get; set; }

    [XmlElement("time")]
    public string ExecutionTime { get; set; }

    [XmlArray("methods")]
    [XmlArrayItem("method")]
    public List<MethodTraceDto> Methods { get; set; } = new();
}