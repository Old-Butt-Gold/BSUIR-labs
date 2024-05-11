using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MIAPR_5;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    Graph _graph;
    Function _separateFunction;
    readonly List<Point>[] _points = new List<Point>[2];
    const int Step = 21;
    
    const int generateNumbers = 250;

    public MainWindow() => InitializeComponent();

    void ClearPoints()
    {
        _points[0] = new();
        _points[1] = new();
    }

    void TextCheck(object sender, TextCompositionEventArgs e)
    {
        var regex = new Regex(@"^-?\d*\,?\d*$");
        if (e.Text == " ") 
            e.Handled = true;
        if (!regex.IsMatch($"{(sender as TextBox)?.Text}{e.Text}")) e.Handled = true;
    }

    private void ButtonTrain_OnClick(object sender, RoutedEventArgs e)
    {
        ClearPoints();
        var potentials = new Potentials();
            
        _points[0].Add(new Point(double.Parse(TextBoxO1X.Text), double.Parse(TextBoxO1Y.Text)));
        _points[0].Add(new Point(double.Parse(TextBoxO2X.Text), double.Parse(TextBoxO2Y.Text)));
        _points[1].Add(new Point(double.Parse(TextBoxO3X.Text), double.Parse(TextBoxO3Y.Text)));
        _points[1].Add(new Point(double.Parse(TextBoxO4X.Text), double.Parse(TextBoxO4Y.Text)));
            
        _separateFunction = potentials.GetFunction(_points);
        TextBoxFunction.Clear();
            
        if (!potentials.Warning)
        {
            TextBoxFunction.Text = _separateFunction.ToString()!;
            _graph = new Graph(_separateFunction, RightBoxBorder.ActualWidth, RightBoxBorder.ActualHeight);
        }
        else
        {
            MessageBox.Show("Невозможно построить разделяющую функцию");
            _graph = new Graph(RightBoxBorder.ActualWidth, RightBoxBorder.ActualHeight);
        }
            
        ButtonClassify.IsEnabled = !potentials.Warning;
        ButtonGenerate.IsEnabled = !potentials.Warning;
            
        Canvas.Source = new DrawingImage(_graph.DrawingGroup);
        for (var i = 0; i < 2; i++)
        {
            for (var j = 0; j < 2; j++)
            {
                _graph.AddPoint(_points[i][j], i == 0);
            }
        }
    }

    private void ButtonClassify_OnClick(object sender, RoutedEventArgs e)
    {
        var testPoint = new Point(double.Parse(TextBoxOX.Text), double.Parse(TextBoxOY.Text));

        var classNumber = _separateFunction.GetValue(testPoint) >= 0 ? 0 : 1;
        _points[classNumber].Add(testPoint);
        _graph.AddPoint(testPoint, classNumber == 0);
        MessageBox.Show($"Класс {classNumber + 1}");
    }

    void Canvas_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (DrawToolTip(e, _points[0], 1)) return;
        if (DrawToolTip(e, _points[1], 2)) return;
            
        bool DrawToolTip(MouseButtonEventArgs e, IEnumerable<Point> list, int classNumber)
        {
            foreach (var point in list)
            {
                var position = e.GetPosition(Canvas);
                if (Math.Abs(point.X * Step + RightBoxBorder.ActualWidth / 2 - position.X) < 10 &&
                    Math.Abs(RightBoxBorder.ActualHeight / 2 - point.Y * Step - position.Y) < 10)
                {
                    MessageBox.Show($"Класс:{classNumber}{Environment.NewLine}({point.X.ToString("F3")};{point.Y.ToString("F3")})");
                    return true;
                }
            }
                
            return false;
        }
    }

    void ButtonGenerate_OnClick(object sender, RoutedEventArgs e)
    {
        var rand = new Random();
        for (int i = 0; i < generateNumbers; i++)
        {
            double x = (rand.NextDouble() * Canvas.ActualWidth - Canvas.ActualWidth / 2) / Step;
            double y = (rand.NextDouble() * Canvas.ActualHeight - Canvas.ActualHeight / 2) / Step;
            Point currentPoint = new Point(x, y);
            var classNumber = _separateFunction.GetValue(currentPoint) >= 0 ? 0 : 1;
            _points[classNumber].Add(currentPoint);
            _graph.AddPoint(currentPoint, classNumber == 0);
        }
    }
}