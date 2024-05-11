namespace MIAPR_7;

public abstract class Rule
{
    private protected const int RANDOM_DELTA = 3;
    
    public readonly ElementType StartElementType;
    public readonly ElementType FirstElementType;
    public readonly ElementType SecondElementType;

    protected Rule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType)
    {
        StartElementType = startElementType;
        FirstElementType = firstElementType;
        SecondElementType = secondElementType;
    }

    public override string ToString()
    {
        return $"Start: {StartElementType.Name} First: {FirstElementType.Name} Second: {SecondElementType.Name}";
    }

    public abstract Element Connect(Element first, Element second);
    public abstract Element TransformConnect(Element first, Element second);
    public abstract bool IsRulePair(Element first, Element second);
}

public class UpRule : Rule
{
    public UpRule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType) 
        : base(startElementType, firstElementType, secondElementType) { }

    public override Element Connect(Element first, Element second)
    {
        var resultLines = new List<Line>(first.Lines);
        resultLines.AddRange(second.Lines);
        return new Element(StartElementType, resultLines, first.StartPosition, second.EndPosition);
    }

    private void MakeSameLength(Element first, Element second)
    {
        var longestElement = GetLongestElement(first, second);
        var shortestElement = GetShortestElement(first, second);

        shortestElement.Resize(longestElement.Length() / shortestElement.Length(), 1.0);
    }

    public override Element TransformConnect(Element first, Element second)
    {
        MakeSameLength(first, second);
        first.Move(0.0, second.StartPosition.Y);
        return Connect(first, second);
    }

    public override bool IsRulePair(Element first, Element second)
    {
        return first.IsSameTypeWith(FirstElementType) && 
               second.IsSameTypeWith(SecondElementType) &&
               second.StartPosition.Y - RANDOM_DELTA < first.EndPosition.Y;
    }

    private Element GetLongestElement(Element first, Element second)
    {
        return first.Length() > second.Length() ? first : second;
    }

    private Element GetShortestElement(Element first, Element second)
    {
        return first.Length() < second.Length() ? first : second;
    }
}

public class LeftRule : Rule
{
    public LeftRule(ElementType startElementType, ElementType firstElementType, ElementType secondElementType) 
        : base(startElementType, firstElementType, secondElementType) { }

    public override Element Connect(Element first, Element second)
    {
        var resultLines = new List<Line>(first.Lines);
        resultLines.AddRange(second.Lines);

        var startY = Math.Max(first.StartPosition.Y, second.StartPosition.Y);
        var endY = Math.Min(first.EndPosition.Y, second.EndPosition.Y);

        var startPosition = new Point(first.StartPosition.X, startY);
        var endPosition = new Point(second.EndPosition.X, endY);

        return new Element(StartElementType, resultLines, startPosition, endPosition);
    }

    public override Element TransformConnect(Element first, Element second)
    {
        second.Move(first.Length(), 0.0);
        return Connect(first, second);
    }

    public override bool IsRulePair(Element first, Element second)
    {
        return first.IsSameTypeWith(FirstElementType) && 
               second.IsSameTypeWith(SecondElementType) &&
               second.StartPosition.Y - RANDOM_DELTA < first.EndPosition.Y;
    }
}