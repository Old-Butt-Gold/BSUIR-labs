using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;

namespace MIAPR_5;

public class Graph
{
    private const int Step = 21;

    private readonly double _width;

    private readonly double _height;

    private readonly GeometryGroup _firstClass;
    private readonly GeometryGroup _secondClass;
    
    private readonly Function _separatingFunction;
    
    public DrawingGroup DrawingGroup { get; private set; }
    
    public Graph(Function separatingFunction, double canvasWidth, double canvasHeight) : this(canvasWidth, canvasHeight)
    {
        _separatingFunction = separatingFunction;
        DrawFunction();
    }

    public Graph(double canvasWidth, double canvasHeight)
    {
        _width = canvasWidth;
        _height = canvasHeight;
        DrawingGroup = new DrawingGroup();
        DrawingGroup.Children.Add(AddAxes());
        _firstClass = AddEmptyClass(Colors.LimeGreen);
        _secondClass = AddEmptyClass(Colors.Aqua);
        
        GeometryGroup AddEmptyClass(Color color)
        {
            var classGroup = new GeometryGroup();
            var brush = new SolidColorBrush(color);
            var geometryDrawing = new GeometryDrawing(brush, new Pen(brush, 5), classGroup);
            DrawingGroup.Children.Add(geometryDrawing);
            return classGroup;
        }
    }
    
    GeometryDrawing AddAxes()
    {
        var axes = new GeometryGroup();
        axes.Children.Add(new LineGeometry(new(0, (double) _height / 2), new(_width, (double) _height / 2)));
        axes.Children.Add(new LineGeometry(new Point((double) _width / 2, 0), new Point((double) _width / 2, _height)));
        var axesBrush = new SolidColorBrush(Colors.White);
        return new GeometryDrawing(axesBrush, new Pen(axesBrush, 2.5), axes);
    }

    public void AddPoint(Point newPoint, bool toFirstClass)
    {
        var currentClass = toFirstClass ? _firstClass : _secondClass;
        currentClass.Children.Add(new EllipseGeometry(new(newPoint.X * Step + (double) _width / 2,
            (double) _height / 2 - newPoint.Y * Step), 3, 3));
    }

    void DrawFunction()
    {
        var functionGeometryGroup = new GeometryGroup();
        var prevPoint = new Point(0, _height / 2.0 - _separatingFunction.GetY(-_width / (2.0 * Step)) * Step);
        for (double x = -_width / (2.0 * Step); x < _width / (2.0 * Step); x += 0.002)
        {
            var nextPoint = new Point(_width / 2.0 + x * Step, _height / 2.0 - _separatingFunction.GetY(x) * Step);
            try
            {
                if (Math.Abs(nextPoint.Y - prevPoint.Y) < _height && IsLineInGraph(prevPoint, nextPoint))
                {
                    functionGeometryGroup.Children.Add(new LineGeometry(prevPoint, nextPoint));
                }
            }
            catch (OverflowException) { }
            prevPoint = nextPoint;
        }

        var functionBrush = new SolidColorBrush(Colors.Chocolate);
        DrawingGroup.Children.Add(new GeometryDrawing(functionBrush, new Pen(functionBrush, 3), functionGeometryGroup));
        
        bool IsLineInGraph(Point nextPoint, Point prevPoint)
        {
            return prevPoint.Y > 0 && prevPoint.Y < _height &&
                   nextPoint.Y > 0 && nextPoint.Y < _height;
        }
    }

    
}