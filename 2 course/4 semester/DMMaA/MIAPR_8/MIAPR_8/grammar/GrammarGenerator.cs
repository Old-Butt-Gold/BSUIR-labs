namespace MIAPR_8.grammar;

public class GrammarGenerator
{
    private readonly List<Rule> _rulesVocabulary = new();
    private int _elementNumber = 1;

    internal Grammar Grammar { get; } = new();

    public GrammarGenerator(List<Element> drawnElements)
    {
        _rulesVocabulary.AddRange(new List<Rule>() { LeftRule.None, UpRule.None});
        Grammar.StartElementType = ConnectElementToGrammar(drawnElements);
    }

    private ElementType ConnectElementToGrammar(List<Element> elements)
    {
        if (elements.Count == 1)
        {
            return elements[0].ElementType;
        }

        RuleSearchResult? resultRule = null;

        foreach (var candidate in elements)
        {
            resultRule = SearchRule(candidate, elements);
            if (resultRule != null)
            {
                break;
            }
        }

        if (resultRule == null)
        {
            throw new InvalidOperationException("Невозможно создать грамматику");
        }

        var result = new ElementType("S" + _elementNumber++);
        Grammar.AddElementType(result);
        Grammar.AddRule(resultRule.GetRule(result));

        return result;
    }

    private RuleSearchResult? SearchRule(Element candidate, List<Element> elements)
    {
        foreach (var rule in _rulesVocabulary)
        {
            if (IsFirstInRule(rule, candidate, elements))
            {
                elements.Remove(candidate);
                var firstElementType = candidate.ElementType;
                var secondElementType = ConnectElementToGrammar(elements);
                return new RuleSearchResult(rule, firstElementType, secondElementType);
            }

            if (IsSecondInRule(rule, candidate, elements))
            {
                elements.Remove(candidate);
                var firstElementType = ConnectElementToGrammar(elements);
                var secondElementType = candidate.ElementType;
                return new RuleSearchResult(rule, firstElementType, secondElementType);
            }
        }

        return null;
    }

    private bool IsFirstInRule(Rule rule, Element candidate, List<Element> elements)
    {
        return elements.TrueForAll(element => !IsDifferentElementInRule(rule, candidate, element));
    }

    private bool IsSecondInRule(Rule rule, Element candidate, List<Element> elements)
    {
        return elements.TrueForAll(element => !IsDifferentElementInRule(rule, element, candidate));
    }

    private bool IsDifferentElementInRule(Rule rule, Element candidate, Element element)
    {
        return !candidate.StartPosition.IsSame(element.StartPosition) &&
               !candidate.EndPosition.IsSame(element.EndPosition) &&
               !rule.IsRulePositionPare(candidate, element);
    }
}

internal class RuleSearchResult(Rule rule, ElementType firstElementType, ElementType secondElementType)
{
    public Rule GetRule(ElementType startElementType) => rule.GetInstance(startElementType, firstElementType, secondElementType);
}