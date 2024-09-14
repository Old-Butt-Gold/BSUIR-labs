namespace SharedComponents;

public class MyPoint
{
    public MyPoint(double x, double y) => (X, Y) = (x, y);
    public MyPoint() : this(0, 0) { }

    public double X { get; set; }
    public double Y { get; set; }
}