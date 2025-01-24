using System.Diagnostics;

namespace Tracer.Core;

public class MethodTrace
{
    public string MethodName { get; }
    public string ClassName { get; }
    public long ExecutionTime { get; private set; }
    public IReadOnlyList<MethodTrace> Methods { get; }

    private readonly Stopwatch _stopwatch;

    public MethodTrace(string methodName, string className)
    {
        MethodName = methodName;
        ClassName = className;
        Methods = new List<MethodTrace>();
        _stopwatch = new Stopwatch();
    }

    public void Start() => _stopwatch.Start();

    public void Stop()
    {
        _stopwatch.Stop();
        ExecutionTime = _stopwatch.ElapsedMilliseconds;
    }

    public void AddNestedMethod(MethodTrace method)
    {
        ((IList<MethodTrace>)Methods).Add(method);
    }
}
