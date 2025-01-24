namespace Tracer.Core;

public class ThreadTrace
{
    public int ThreadId { get; }
    public long TotalExecutionTime => Methods.Sum(m => m.ExecutionTime);
    public IReadOnlyList<MethodTrace> Methods { get; }

    public ThreadTrace(int threadId, List<MethodTrace> methods)
    {
        ThreadId = threadId;
        Methods = new List<MethodTrace>(methods);
    }
}