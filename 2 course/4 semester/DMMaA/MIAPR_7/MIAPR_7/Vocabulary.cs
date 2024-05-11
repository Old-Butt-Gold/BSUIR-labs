namespace MIAPR_7;

public class ElementType
{
    public string Name { get; }
        
    public ElementType(string name) => Name = name;

    public virtual bool IsTerminal() => false;

    public bool IsSame(ElementType compared) => Name == compared.Name;
}

public class TerminalElementType : ElementType
{
    private readonly Line _standardLine;

    public TerminalElementType(string name, Line standardLine) : base(name)
    {
        _standardLine = standardLine;
    }

    public override bool IsTerminal() => true;

    public Element GetStandardElement()
    {
        var lineCopy = new Line(
            new Point(_standardLine.Start.X, _standardLine.Start.Y),
            new Point(_standardLine.End.X, _standardLine.End.Y)
        );
        return new Element(this, lineCopy);
    }
}