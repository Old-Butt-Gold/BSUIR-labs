using System.Windows;
using System.Windows.Input;

namespace MIAPR_7;

public partial class MainWindow : Window
{
    List<Line> _drawedLines = new();
    List<Element> _drawedElements = new();
    Point? _from = null;

    Drawer _drawer;
    ElementGenerator _elementGenerator;
    
    public MainWindow()
    {
        InitializeComponent();
        _drawer = new Drawer(Canvas);
        _drawer.CleanCanvas();

        _elementGenerator = new();
        
    }

    void Clean()
    {
        _drawer.CleanCanvas();
        _drawedElements.Clear();
        _drawedLines.Clear();
        ResultLabel.Content = "";
    }
    
    private void Generate_Click(object sender, RoutedEventArgs e)
    {
        Clean();
        var element = _elementGenerator.GenerateElement();
        element.Lines = element.Lines.Distinct().ToList(); //Чтобы избежать дублирования
        
        _drawer.Draw(element);
        
        foreach (var line in element.Lines)
        {
            _drawedLines.Add(line);
            var terminalElement = _elementGenerator.GetTerminalElement(line);
            _drawedElements.Add(terminalElement);
        }
    }

    void Validate_Click(object sender, RoutedEventArgs e)
    {
        ResultLabel.Content = _elementGenerator.IsImageCorrect(_drawedElements) ? "Image is correct" : "Image is not correct";
    }

    private void Clean_Click(object sender, RoutedEventArgs e)
    {
        Clean();
    }

    void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (_from == null)
        {
            var temp = e.GetPosition(Canvas);
            _from = new(temp.X, temp.Y);
        }
        else
        {
            var to = e.GetPosition(Canvas);
            var line = new Line(_from, new(to.X, to.Y));
            _drawer.DrawLine(line);
            _drawedLines.Add(line);

            var factFrom = _drawer.GetFactPoint(_from);
            var factTo = _drawer.GetFactPoint(new(to.X, to.Y));
            var lineSecond = new Line(factFrom, factTo);
            var drewElement = _elementGenerator.GetTerminalElement(lineSecond);
            _drawedElements.Add(drewElement);

            _from = null;
        }
    }

    private void Canvas_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
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
}