using System.Windows.Media;
using System.Xml.Serialization;

namespace SharedComponents.AbstractClasses;

public class AbstractShapeXML
{
    public object TagShape { get; set; }

    public int Angle { get; set; }

    public MyPoint TopLeft { get; set; }
    
    public MyPoint DownRight { get; set; }
    
    public double StrokeThickness { get; set; }
    
    [XmlIgnore]
    public Brush BackgroundColor { get; set; }

    [XmlIgnore]
    public Brush PenColor { get; set; }

    //For XML
    public string BackgroundColorString
    {
        get => BackgroundColor.ToString();
        set => BackgroundColor = (SolidColorBrush)new BrushConverter().ConvertFromString(value);
    }

    public string PenColorString
    {
        get => PenColor.ToString();
        set => PenColor = (SolidColorBrush)new BrushConverter().ConvertFromString(value);
    }
}