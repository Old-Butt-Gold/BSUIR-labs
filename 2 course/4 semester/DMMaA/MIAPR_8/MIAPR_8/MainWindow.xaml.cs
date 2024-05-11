using System.Windows;
using System.Windows.Input;
using MIAPR_8.grammar;
using Point = MIAPR_8.grammar.Point;

namespace MIAPR_8;

public partial class MainWindow : Window
{
    Drawer _drawer;
    Grammar? _grammar;
    Point? _from;
    
    List<Line> _drawedLines = new();
    List<Element> _drawedElements = new();

    public MainWindow()
    {
        InitializeComponent();
        _drawer = new(Canvas);
    }

    void Generate_Click(object sender, RoutedEventArgs e)
    {
        _drawer.CleanCanvas();
        if (_grammar != null)
        {
            Element element = _grammar.GenerateElement();
            element.Lines = element.Lines.Where(x =>
                !double.IsNaN(x.From.X) && !double.IsNaN(x.From.Y) && !double.IsNaN(x.To.X) && !double.IsNaN(x.To.Y)).ToList();
            _drawer.Draw(element);

            _drawedElements.Clear();
            _drawedLines.Clear();
            foreach (var line in element.Lines)
            {
                Element terminal = Grammar.GetTerminalElement(line);
                _drawedElements.Add(terminal);
                _drawedLines.Add(line);
            }
        }
        else
        {
            ResultLabel.Content = "Нет грамматики";
        }
    }

    void Clean()
    {
        _drawer.CleanCanvas();
        ResultLabel.Content = "";
        GrammarLabel.Content = "";
        _grammar = null;
        _drawedLines.Clear();
        _drawedElements.Clear();
    }

    void Synthesize_Click(object sender, RoutedEventArgs e)
    {
        var listCopy = _drawedElements.Select(element => element).ToList();
        try
        {
            listCopy.Reverse();
            var generator = new GrammarGenerator(listCopy);
            _grammar = generator.Grammar;
            GrammarLabel.Content = generator.Grammar.ToString();
            ResultLabel.Content = "";
        }
        catch (Exception ex)
        {
            ResultLabel.Content = ex.Message;
            GrammarLabel.Content = "";
            _grammar = null;
        }      
    }

    void Clean_Click(object sender, RoutedEventArgs e) => Clean();

    void Canvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_from == null)
        {
            var temp = e.GetPosition(Canvas);
            _from = new(temp.X, temp.Y);
        }
        else
        {
            if (Canvas.Children.Count > 0) 
                Canvas.Children.RemoveAt(Canvas.Children.Count - 1);
            var temp = e.GetPosition(Canvas);
            Point to = new(temp.X, temp.Y);
            _drawer.DrawLine(_from, to);
            _drawedLines.Add(new Line(_from, to));

            Point factFrom = _drawer.GetFactPoint(_from);
            Point factTo = _drawer.GetFactPoint(to);
            Line line = new Line(factFrom, factTo);
            Element drawnElement = Grammar.GetTerminalElement(line);
            _drawedElements.Add(drawnElement);

            _from = null;
        }
    }

    void Canvas_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        _from = null;
        if (Canvas.Children.Count > 0)
        {
            Canvas.Children.RemoveAt(Canvas.Children.Count - 1);
        }

        if (_drawedElements.Count > 0)
        {
            _drawedElements.RemoveAt(_drawedElements.Count - 1);
        }

        if (_drawedLines.Count > 0)
        {
            _drawedLines.RemoveAt(_drawedLines.Count - 1);
        }
    }

    void Border_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_from != null)
        {
            _drawer.CleanCanvas();
            foreach (var line in _drawedLines)
            {
                _drawer.DrawLine(line);
            }
            
            _drawer.DrawLine(_from, new Point(e.GetPosition(Canvas).X, e.GetPosition(Canvas).Y));
        }
    }

    void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}