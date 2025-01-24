using Tracer.Core;
using Tracer.Serialization.Xml.Dto;

namespace Tracer.Serialization.Xml;

public static class TraceResultConverter
{
    public static TraceResultDto ToDto(TraceResult traceResult)
    {
        return new TraceResultDto
        {
            Threads = traceResult.Threads.Select(t => new ThreadTraceDto
            {
                ThreadId = t.ThreadId,
                TotalExecutionTime = $"{t.Methods.Sum(m => m.ExecutionTime)}ms",
                Methods = t.Methods.Select(x => ToDto(x)).ToList()
            }).ToList()
        };
    }

    private static MethodTraceDto ToDto(MethodTrace method)
    {
        return new MethodTraceDto
        {
            MethodName = method.MethodName,
            ClassName = method.ClassName,
            ExecutionTime = $"{method.ExecutionTime}ms",
            Methods = method.Methods.Select(x => ToDto(x)).ToList()
        };
    }
}