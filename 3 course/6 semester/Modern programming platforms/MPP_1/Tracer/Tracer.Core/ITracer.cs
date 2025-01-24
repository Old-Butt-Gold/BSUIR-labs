namespace Tracer.Core;

public interface ITracer
{
    // Called at the start of the traced method
    void StartTrace();

    // Called at the end of the traced method
    void StopTrace();

    // Retrieve the trace results
    TraceResult GetTraceResult();
}