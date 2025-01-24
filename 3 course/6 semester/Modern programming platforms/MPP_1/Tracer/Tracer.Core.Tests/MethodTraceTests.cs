namespace Tracer.Core.Tests;

public class MethodTraceTests
{
    [Theory]
    [InlineData("TestMethod", "TestClass", 100)]
    [InlineData("TestMethod", "TestClass", 1)]
    public void StartStop_ShouldSetExecutionTime(string methodName, string className, int sleep)
    {
        // Arrange
        var methodTrace = new MethodTrace(methodName, className);
        
        // Act
        methodTrace.Start();
        Thread.Sleep(sleep);
        methodTrace.Stop();
        
        // Assert
        Assert.True(methodTrace.ExecutionTime >= sleep);
    }
    
    [Theory]
    [InlineData("ParentMethod", "TestClass", "ChildMethod", "TestClass")]
    public void AddNestedMethod_ShouldAddNestedTrace(string parentMethodName, string parentClassName, string childMethodName, string childClassName)
    {
        var parentTrace = new MethodTrace(parentMethodName, parentClassName);
        var childTrace = new MethodTrace(childMethodName, childClassName);

        parentTrace.AddNestedMethod(childTrace);

        Assert.Single(parentTrace.Methods); 
        Assert.Equal(childTrace, parentTrace.Methods[0]); 
        Assert.Equal(childClassName, parentTrace.Methods[0].ClassName); 
        Assert.Equal(childMethodName, parentTrace.Methods[0].MethodName); 
    }

}