namespace MIAPR_8.grammar;

public class ElementType
{
    public static readonly ElementType None = new("NONE");
    public string Name { get; }

    public ElementType(string name) => Name = name;

    public virtual bool IsTerminal() => false;
}

public class TerminalElementType : ElementType
{
    private readonly Line _standardLine;

    public TerminalElementType(string name, Line standardLine) : base(name)
    {
        _standardLine = standardLine;
    }

    public override bool IsTerminal() => true;

    public Element StandardElement => new(
        this,
        new Line(
            new Point(_standardLine.From.X, _standardLine.From.Y),
            new Point(_standardLine.To.X, _standardLine.To.Y)
        )
    );
}