namespace Tracer.Core.Tests;

public class TracerTests
{
    [Fact]
    public void StartStopTrace_ShouldRecordMethodExecution()
    {
        var tracer = new Tracer();

        tracer.StartTrace();
        Thread.Sleep(100);
        tracer.StopTrace();

        var result = tracer.GetTraceResult();

        Assert.Single(result.Threads);
        var threadTrace = result.Threads[0];
        Assert.Single(threadTrace.Methods);
        Assert.True(threadTrace.Methods[0].ExecutionTime >= 100);
    }

    [Fact]
    public void StartTrace_ShouldHandleNestedCalls()
    {
        var tracer = new Tracer();

        tracer.StartTrace();
        tracer.StartTrace();
        Thread.Sleep(100);
        tracer.StopTrace();
        tracer.StopTrace();

        var result = tracer.GetTraceResult();

        var threadTrace = result.Threads[0];
        Assert.Single(threadTrace.Methods);
        Assert.Single(threadTrace.Methods[0].Methods);
    }
    
    [Fact]
    public void Tracer_ShouldHandleMultipleThreads()
    {
        var tracer = new Tracer();

        void TraceInThread()
        {
            tracer.StartTrace();
            Thread.Sleep(50);
            tracer.StopTrace();
        }

        var threads = new Thread[3];

        // Act
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(TraceInThread);
            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        var result = tracer.GetTraceResult();

        Assert.Equal(3, result.Threads.Count);

        foreach (var threadTrace in result.Threads)
        {
            Assert.Single(threadTrace.Methods);
            Assert.True(threadTrace.Methods[0].ExecutionTime >= 50);
        }
    }
}