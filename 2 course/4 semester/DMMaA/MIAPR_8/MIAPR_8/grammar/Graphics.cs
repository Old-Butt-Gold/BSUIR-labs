namespace MIAPR_8.grammar;

public class Point
{
    public static readonly Point None = new Point(double.NaN, double.NaN);

    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    public void Move(double deltaX, double deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }

    public bool IsSame(Point other)
    {
        return Math.Abs(other.X - X) < 0.01 && Math.Abs(other.Y - Y) < 0.01;
    }
}

public class Line
{
    public Point From { get; set; }
    public Point To { get; set; }
    
    public Line(Point from, Point to)
    {
        From = from;
        To = to;
    }

    public void Move(double deltaX, double deltaY)
    {
        From.Move(deltaX, deltaY);
        To.Move(deltaX, deltaY);
    }

    public void Resize(double xScale, double yScale, Point centralPoint)
    {
        double xDelta = (To.X - From.X) * xScale;
        double yDelta = (To.Y - From.Y) * yScale;

        double xStartDelta = (From.X - centralPoint.X) * xScale;
        double yStartDelta = (From.Y - centralPoint.Y) * yScale;

        From = new Point(centralPoint.X + xStartDelta, centralPoint.Y + yStartDelta);
        To = new Point(From.X + xDelta, From.Y + yDelta);
    }
}

public class Element
{
    public ElementType ElementType { get; }
    public List<Line> Lines { get; set; }
    public Point StartPosition { get; set; }
    public Point EndPosition { get; set; }

    public Element(ElementType elementType)
    {
        ElementType = elementType;
        Lines = new List<Line>();
        StartPosition = Point.None;
        EndPosition = Point.None;
    }

    public Element(ElementType elementType, Line line) : this(elementType)
    {
        Lines.Add(line);

        double xFrom = line.From.X;
        double xTo = line.To.X;
        double yFrom = line.From.Y;
        double yTo = line.To.Y;
        StartPosition = new Point(Math.Min(xFrom, xTo), Math.Max(yFrom, yTo));
        EndPosition = new Point(Math.Max(xFrom, xTo), Math.Min(yFrom, yTo));
    }

    public Element(ElementType elementType, List<Line> lines, Point startPosition, Point endPosition) : this(elementType)
    {
        Lines.AddRange(lines);
        StartPosition = startPosition;
        EndPosition = endPosition;
    }

    public double Length => Math.Abs(EndPosition.X - StartPosition.X);

    public void Move(double xDelta, double yDelta)
    {
        StartPosition.Move(xDelta, yDelta);
        EndPosition.Move(xDelta, yDelta);
        foreach (var line in Lines)
        {
            line.Move(xDelta, yDelta);
        }
    }

    public void Resize(double xScale, double yScale)
    {
        double deltaX = (EndPosition.X - StartPosition.X) * xScale;
        double deltaY = (EndPosition.Y - StartPosition.Y) * yScale;

        EndPosition = new Point(StartPosition.X + deltaX, StartPosition.Y + deltaY);
        foreach (var line in Lines)
        {
            line.Resize(xScale, yScale, StartPosition);
        }
    }
}