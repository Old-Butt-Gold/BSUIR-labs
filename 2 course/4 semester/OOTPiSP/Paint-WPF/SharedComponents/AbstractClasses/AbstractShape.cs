using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;
using Newtonsoft.Json;
using SharedComponents.Interfaces;

namespace SharedComponents.AbstractClasses;

[Serializable]
public abstract class AbstractShape : IDataErrorInfo, INotifyPropertyChanged
{
    [JsonIgnore]
    [XmlIgnore]
    public static Canvas Canvas { get; set; }
    
    int _angle;
    double _strokeThickness = 1;
    Brush _backgroundColor;
    Brush _penColor;
    MyPoint _topLeft;
    MyPoint _downRight;

    [JsonIgnore]
    public IDrawStrategy DrawStrategy { get; protected set; }

    [JsonIgnore] 
    public int CanvasIndex { get; set; } = -1;

    [JsonIgnore]
    public virtual object TagShape { get; }

    public int Angle
    {
        get => _angle;
        set
        {
            if (value is > -360 and < 360)
            {
                _angle = value;
                OnPropertyChanged();
                DrawAlgorithm();
                _errors["Angle"] = null;
            }
            else
            {
                _errors["Angle"] = "Угол должен быть в диапазоне от -360 до 360!";
            }
        }
    }

    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            if (value is >= 0 and <= 72)
            {
                _strokeThickness = value;
                OnPropertyChanged();
                DrawAlgorithm();
                _errors["StrokeThickness"] = null;
            }
            else
            {
                _errors["StrokeThickness"] = "Толщина карандаша должна быть от 0.0 до 72.0!";
            }
        }
    }

    public MyPoint TopLeft
    {
        get => _topLeft;
        set
        {
            _topLeft = value;
            OnPropertyChanged();
            RecalculateCornerOxy(_topLeft, _downRight);
        }
    }

    public MyPoint DownRight
    {
        get => _downRight;
        set
        {
            _downRight = value;
            OnPropertyChanged();
            RecalculateCornerOxy(_topLeft, _downRight);
        }
    }

    [JsonIgnore]
    public int CornerOXY { get; private set; }

    public Brush BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            DrawAlgorithm();
            OnPropertyChanged();
        }
    }

    public Brush PenColor
    {
        get => _penColor;
        set
        {
            _penColor = value;
            DrawAlgorithm();
            OnPropertyChanged();
        }
    }

    protected AbstractShape(MyPoint topLeft, MyPoint downRight, Brush bgColor, Brush penColor, int angle)
    {
        _backgroundColor = bgColor;
        _penColor = penColor;

        _topLeft = topLeft;
        _downRight = downRight;

        RecalculateCornerOxy(_topLeft, _downRight);
        
        _angle = angle % 360;
    }

    public void DrawAlgorithm()
    {
        var drawnShape = DrawStrategy.Draw(this);

        if (drawnShape != null)
        {
            if (CanvasIndex < 0)
            {
                CanvasIndex = Canvas.Children.Count;
                Canvas.Children.Add(drawnShape);
            }
            else
            {
                Canvas.Children.RemoveAt(CanvasIndex);
                Canvas.Children.Insert(CanvasIndex, drawnShape);
            }
            drawnShape.Tag = CanvasIndex;
        }
    }

    void RecalculateCornerOxy(MyPoint start, MyPoint end)
    {
        //X увеличивается вправо; Y увеличивает вниз (0; 0) – левый верхний угол
        if (end.X > start.X)
        {
            CornerOXY = end.Y > start.Y ? 4 : 1;
        }
        else
        {
            CornerOXY = end.Y > start.Y ? 3 : 2;
        }
    }

    #region IDataErrorInfo Members

    [JsonIgnore]
    [XmlIgnore]
    public string Error => string.Empty;

    private readonly Dictionary<string, string> _errors = new();

    [JsonIgnore]
    public bool IsValid => _errors.Values.All(x => x == null);

    [JsonIgnore]
    public IEnumerable<string> GetErrors => _errors.Values.Where(x => x != null);

    [JsonIgnore]
    [XmlIgnore]
    //Для валидации
    public string this[string columnName] => _errors.GetValueOrDefault(columnName);

    #endregion

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}