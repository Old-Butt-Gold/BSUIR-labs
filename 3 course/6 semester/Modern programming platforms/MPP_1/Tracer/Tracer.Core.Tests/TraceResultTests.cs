namespace Tracer.Core.Tests;

public class TraceResultTests
{
    [Fact]
    public void Constructor_ShouldStoreThreads()
    {
        var threads = new List<ThreadTrace>
        {
            new ThreadTrace(1, new List<MethodTrace>()),
            new ThreadTrace(2, new List<MethodTrace>())
        };

        var traceResult = new TraceResult(threads);

        var storedThreads = traceResult.Threads;

        Assert.Equal(threads.Count, storedThreads.Count);
        Assert.Equal(threads[0], storedThreads[0]);
        Assert.Equal(threads[1], storedThreads[1]);
    }
}