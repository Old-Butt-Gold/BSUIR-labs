using Xunit.Abstractions;

namespace Tracer.Core.Tests;

public class ThreadTraceTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ThreadTraceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TotalExecutionTime_ShouldSumExecutionTimes()
    {
        var methods = new List<MethodTrace>();
        var method = new MethodTrace("Method1", "TestClass");
        method.Start();
        Thread.Sleep(100);
        method.Stop();
        methods.Add(method);
        
        method = new MethodTrace("Method2", "TestClass");
        method.Start();
        Thread.Sleep(200);
        method.Stop();
        methods.Add(method);

        var threadTrace = new ThreadTrace(1, methods);


        var totalExecutionTime = threadTrace.TotalExecutionTime;
        
        _testOutputHelper.WriteLine(totalExecutionTime.ToString());
        Assert.True(totalExecutionTime >= 300);
        Assert.Equal(1, threadTrace.ThreadId);
    }
}