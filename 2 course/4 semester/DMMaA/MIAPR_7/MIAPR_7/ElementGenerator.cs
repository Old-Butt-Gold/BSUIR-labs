using System.Text;
using System.Windows;

namespace MIAPR_7;

public class ElementGenerator
{
    //terminals
    private const string HORIZONTAL_LINE = "horizontal";
    private const string VERTICAL_LINE = "vertical";
    private const string RIGHT_45_DEG = "right45";
    private const string LEFT_45_DEG = "left45";
    
    //non-terminals
    private const string LEFT_BRANCH = "l_branch";
    private const string RIGHT_BRANCH = "r_branch";
    private const string BRANCH_LAYER = "layer";
    private const string TREE = "tree";
    private const string START_S = "big_tree";
    
    readonly Dictionary<string, ElementType> _dictionary;
    readonly Dictionary<string, Rule> _rules;
    readonly ElementType _startElementType;

    public ElementGenerator()
    {
        _dictionary = CreateDictionary();
        _rules = CreateRules();
        _startElementType = new ElementType(START_S);
    }

    Dictionary<string, ElementType> CreateDictionary()
    {
        return new Dictionary<string, ElementType>
        {
            {HORIZONTAL_LINE, new TerminalElementType(HORIZONTAL_LINE, new Line(new Point(0.0, 0.0), new Point(10.0, 0.0)))},
            {VERTICAL_LINE, new TerminalElementType(VERTICAL_LINE, new Line(new Point(0.0, 0.0), new Point(0.0, 10.0)))},
            {RIGHT_45_DEG, new TerminalElementType(RIGHT_45_DEG, new Line(new Point(0.0, 0.0), new Point(10.0, 10.0)))},
            {LEFT_45_DEG, new TerminalElementType(LEFT_45_DEG, new Line(new Point(10.0, 0.0), new Point(0.0, 10.0)))},
            {LEFT_BRANCH, new ElementType(LEFT_BRANCH)},
            {RIGHT_BRANCH, new ElementType(RIGHT_BRANCH)},
            {BRANCH_LAYER, new ElementType(BRANCH_LAYER)},
            {TREE, new ElementType(TREE)},
            {START_S, new ElementType(START_S)}
        };
    }

    Dictionary<string, Rule> CreateRules()
    {
        return new Dictionary<string, Rule>
        {
            {LEFT_BRANCH, new LeftRule(_dictionary[LEFT_BRANCH], _dictionary[RIGHT_45_DEG], _dictionary[VERTICAL_LINE])},
            {RIGHT_BRANCH, new LeftRule(_dictionary[RIGHT_BRANCH], _dictionary[VERTICAL_LINE], _dictionary[LEFT_45_DEG])},
            {BRANCH_LAYER, new LeftRule(_dictionary[BRANCH_LAYER], _dictionary[LEFT_BRANCH], _dictionary[RIGHT_BRANCH])},
            {TREE, new UpRule(_dictionary[TREE], _dictionary[BRANCH_LAYER], _dictionary[BRANCH_LAYER])},
            {START_S, new UpRule(_dictionary[START_S], _dictionary[TREE], _dictionary[HORIZONTAL_LINE])}
        };
    }

    public Element GetTerminalElement(Line line)
    {
        string elementName = GetTerminalElementName(line);
        return new Element(_dictionary[elementName], line);
    }

    private string GetTerminalElementName(Line line)
    {
        double deltaX = line.Start.X - line.End.X;
        double deltaY = line.Start.Y - line.End.Y;
        if (Math.Abs(deltaY) < 1)
        {
            return HORIZONTAL_LINE;
        }
        if (Math.Abs(deltaX) < 1)
        {
            return VERTICAL_LINE;
        }

        if (Math.Abs(deltaX / deltaY) < 0.2)
        {
            return VERTICAL_LINE;
        }

        if (Math.Abs(deltaY / deltaX) < 0.2)
        {
            return HORIZONTAL_LINE;
        }
        Point highPoint = line.End.Y > line.Start.Y ? line.End : line.Start;
        Point lowPoint = line.End.Y < line.Start.Y ? line.End : line.Start;
        return highPoint.X < lowPoint.X ? LEFT_45_DEG : RIGHT_45_DEG;
    }

    public Element GenerateElement(ElementType elementType = null)
    {
        elementType ??= _startElementType;

        if (elementType.IsTerminal())
        {
            return (elementType as TerminalElementType)!.GetStandardElement();
        }

        Rule rule = _rules[elementType.Name];
        return rule.TransformConnect(
            GenerateElement(rule.FirstElementType), GenerateElement(rule.SecondElementType)
        );
    }

    public bool IsImageCorrect(List<Element> elements)
    {
        List<Line> correctLines = GenerateElement().Lines;
        correctLines = correctLines.Distinct().ToList(); //Ибо эта ебучая хуйня дублирует вертикальную линию после рекурсии
        List<Element> correctElements = correctLines.ConvertAll(GetTerminalElement);

        if (correctElements.Count != elements.Count)
        {
            var missingElements = correctElements
                .Where(correctElement => !elements.Any(element => element.IsSameTypeWith(correctElement.ElementType)))
                .Select(element => element.ElementType.Name);

            MessageBox.Show("Не найдено: " + string.Join("; ", missingElements));
            return false;
        }

        var correctElementTypes = new HashSet<ElementType>(correctElements.Select(element => element.ElementType));

        foreach (var element in elements)
        {
            if (!correctElementTypes.Contains(element.ElementType))
            {
                MessageBox.Show("Не найден элемент: " + element.ElementType.Name);
                return false;
            }
        }

        return true;
    }
}