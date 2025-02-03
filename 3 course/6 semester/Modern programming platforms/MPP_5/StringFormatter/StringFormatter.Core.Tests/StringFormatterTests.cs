namespace StringFormatter.Core.Tests;

public class StringFormatterTests
{
    private readonly StringFormatter _formatter = new();
    
    private class TestClass
    {
        public string Name { get; set; } = "Alice";
        public int Age { get; set; } = 25;
        public NestedClass Nested { get; set; } = new();

        public int[] Array { get; set; } = [1, 2, 3];
        public int[][] ArrayNested { get; set; } = [[1, 2, 3]];

        public int FieldValue = 2;
    }
    
    private class NestedClass
    {
        public string Value { get; set; } = "NestedValue";
    }

    [Fact]
    public void Format_ShouldReplace_PlaceholdersWithValues()
    {
        var obj = new TestClass();
        string template = "Hello, {Name}! You are {Age} years old.";
        string result = _formatter.Format(template, obj);
        
        Assert.Equal("Hello, Alice! You are 25 years old.", result);
    }

    [Fact]
    public void Format_ShouldHandle_NestedProperties()
    {
        var obj = new TestClass();
        string template = "Nested value: {Nested.Value}";
        string result = _formatter.Format(template, obj);
        
        Assert.Equal("Nested value: NestedValue", result);
    }

    [Fact]
    public void Format_ShouldHandle_ArrayValues()
    {
        var obj = new TestClass();
        string template = "Array[0] value: {Array[0]}";
        string result = _formatter.Format(template, obj);
        
        Assert.Equal("Array[0] value: 1", result);
    }
    
    [Fact]
    public void Format_ShouldHandle_ArrayNestedValues()
    {
        var obj = new TestClass();
        string template = "ArrayNested[0][0] value: {ArrayNested[0][0]}";
        string result = _formatter.Format(template, obj);
        
        Assert.Equal("ArrayNested[0][0] value: 1", result);
    }
    
    [Fact]
    public void Format_ShouldThrow_ForInvalidIndex()
    {
        var obj = new TestClass();
        string template = "Array[0][0] value: {Array[0][0]}";
        
        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }
    
    [Fact]
    public void Format_ShouldThrow_ForFieldValues()
    {
        var obj = new TestClass();
        
        string template = "FieldValue value: {FieldValue}";
        
        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }

    [Fact]
    public void Format_ShouldThrow_ForUnbalancedBraces()
    {
        var obj = new TestClass();
        string template1 = "Hello {Name";
        string template2 = "Hello }Name{";
        
        Assert.Throws<FormatException>(() => _formatter.Format(template1, obj));
        Assert.Throws<FormatException>(() => _formatter.Format(template2, obj));
    }

    [Fact]
    public void Format_ShouldHandle_EscapedBraces()
    {
        var obj = new TestClass();
        string template = "{{Hello}} {Name}";
        string result = _formatter.Format(template, obj);
        
        Assert.Equal("{Hello} Alice", result);
    }

    [Fact]
    public void Format_ShouldThrow_IfPropertyNotFound()
    {
        var obj = new TestClass();
        string template = "{NonExistent}";
        
        Assert.Throws<FormatException>(() => _formatter.Format(template, obj));
    }

    [Fact]
    public void Format_ShouldThrow_IfTargetIsNull()
    {
        string template = "{Name}";
        Assert.Throws<ArgumentNullException>(() => _formatter.Format(template, null!));
    }

    [Fact]
    public void Format_ShouldThrow_IfTemplateIsNull()
    {
        var obj = new TestClass();
        Assert.Throws<ArgumentNullException>(() => _formatter.Format(null!, obj));
    }
}