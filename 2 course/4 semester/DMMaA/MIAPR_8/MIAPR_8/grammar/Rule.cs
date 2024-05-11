namespace MIAPR_8.grammar;

internal abstract class Rule
{
    private protected const int RANDOM_DELTA = 3;

    public ElementType StartElementType { get; }
    public ElementType FirstElementType { get; }
    public ElementType SecondElementType { get; }

    protected Rule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType)
    {
        StartElementType = startElementType;
        FirstElementType = firstElementType;
        SecondElementType = secondElementType;
    }

    public abstract Element Connect(Element first, Element second);

    public abstract Element TransformConnect(Element first, Element second);

    public abstract bool IsRulePositionPare(Element first, Element second);

    public abstract Rule GetInstance(ElementType startElementType, ElementType firstElementType, ElementType secondElementType);

    public abstract string Name { get; }
}

internal class UpRule : Rule
{
    public static readonly UpRule None = new UpRule(ElementType.None, ElementType.None, ElementType.None);

    public UpRule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType)
        : base(startElementType, firstElementType, secondElementType)
    { }

    public override Element Connect(Element first, Element second)
    {
        List<Line> resultLines = new List<Line>(first.Lines);
        resultLines.AddRange(second.Lines);
        return new Element(StartElementType, resultLines, first.StartPosition, second.EndPosition);
    }

    public override Element TransformConnect(Element first, Element second)
    {
        MakeSameLength(first, second);
        first.Move(0.0, second.StartPosition.Y);
        return Connect(first, second);
    }

    public override bool IsRulePositionPare(Element first, Element second) => second.StartPosition.Y - RANDOM_DELTA < first.EndPosition.Y;

    public override Rule GetInstance(ElementType startElementType, ElementType firstElementType, ElementType secondElementType) => new UpRule(startElementType, firstElementType, secondElementType);

    public override string Name => "Up";

    private void MakeSameLength(Element first, Element second)
    {
        Element longestElement = GetLongestElement(first, second);
        Element shortestElement = GetShortestElement(first, second);
        shortestElement.Resize(longestElement.Length / shortestElement.Length, 1.0);
    }

    Element GetLongestElement(Element first, Element second) => first.Length > second.Length ? first : second;

    Element GetShortestElement(Element first, Element second) => first.Length < second.Length ? first : second;
}

internal class LeftRule : Rule
{
    public static readonly LeftRule None = new LeftRule(ElementType.None, ElementType.None, ElementType.None);

    public LeftRule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType)
        : base(startElementType, firstElementType, secondElementType)
    {
    }

    public override Element Connect(Element first, Element second)
    {
        List<Line> resultLines = new List<Line>(first.Lines);
        resultLines.AddRange(second.Lines);
        double startY = Math.Max(first.StartPosition.Y, second.StartPosition.Y);
        double endY = Math.Min(first.EndPosition.Y, second.EndPosition.Y);
        Point startPosition = new Point(first.StartPosition.X, startY);
        Point endPosition = new Point(second.EndPosition.X, endY);
        return new Element(StartElementType, resultLines, startPosition, endPosition);
    }

    public override Element TransformConnect(Element first, Element second)
    {
        second.Move(first.Length, 0.0);
        return Connect(first, second);
    }

    public override bool IsRulePositionPare(Element first, Element second)
    {
        return first.EndPosition.X - RANDOM_DELTA < second.StartPosition.X;
    }

    public override Rule GetInstance(ElementType startElementType, ElementType firstElementType, ElementType secondElementType)
    {
        return new LeftRule(startElementType, firstElementType, secondElementType);
    }

    public override string Name => "Left";
}