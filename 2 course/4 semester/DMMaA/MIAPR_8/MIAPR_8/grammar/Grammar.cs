using System.Text;

namespace MIAPR_8.grammar;

internal class Grammar
{
    private readonly Dictionary<string, Rule> _rules = new();
    public ElementType StartElementType { get; set; }

    static readonly Dictionary<string, ElementType> Dictionary = new()
    {
        { "horizontal", new TerminalElementType("horizontal", new Line(new Point(0.0, 0.0), new Point(10.0, 0.0))) },
        { "vertical", new TerminalElementType("vertical", new Line(new Point(0.0, 0.0), new Point(0.0, 10.0))) },
        { "right45", new TerminalElementType("right45", new Line(new Point(0.0, 0.0), new Point(10.0, 10.0))) },
        { "left45", new TerminalElementType("left45", new Line(new Point(10.0, 0.0), new Point(0.0, 10.0))) }
    };

    public static Element GetTerminalElement(Line line)
    {
        string elementName = GetTerminalElementName(line);
        ElementType elementType = Dictionary[elementName];
        return new Element(elementType, line);
    }

    private static string GetTerminalElementName(Line line)
    {
        double deltaX = line.From.X - line.To.X;
        double deltaY = line.From.Y - line.To.Y;
        if (Math.Abs(deltaY) < 1)
        {
            return "horizontal";
        }
        if (Math.Abs(deltaX) < 1)
        {
            return "vertical";
        }

        if (Math.Abs(deltaX / deltaY) < 0.2)
        {
            return "vertical";
        }

        if (Math.Abs(deltaY / deltaX) < 0.2)
        {
            return "horizontal";
        }
        Point highPoint = line.To.Y > line.From.Y ? line.To : line.From;
        Point lowPoint = line.To.Y < line.From.Y ? line.To : line.From;
        return highPoint.X < lowPoint.X ? "right45" : "left45";
    }

    public Element GenerateElement() => GenerateElement(StartElementType);

    private Element GenerateElement(ElementType elementType)
    {
        if (elementType.IsTerminal())
        {
            return (elementType as TerminalElementType)!.StandardElement;
        }
        Rule rule = _rules[elementType.Name];
        return rule.TransformConnect(
            GenerateElement(rule.FirstElementType), GenerateElement(rule.SecondElementType)
        );
    }

    public void AddElementType(ElementType elementType) => Dictionary[elementType.Name] = elementType;

    public void AddRule(Rule rule) => _rules[rule.StartElementType.Name] = rule;

    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        foreach (Rule rule in _rules.Values)
        {
            result.Append($"{rule.StartElementType.Name} -> {rule.Name}({rule.FirstElementType.Name}, {rule.SecondElementType.Name});\n");
        }
        return result.ToString();
    }
}