namespace MIAPR_7;

public class Point : IEquatable<Point>
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y) => (X, Y) = (x, y);

    public void Move(double deltaX, double deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }

    public static Point None { get; } = new Point(0.0, 0.0);
    
    public bool Equals(Point? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Point)obj);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);
}

public class Line : IEquatable<Line>
{
    public override string ToString() => $"Start: {Start.X}:{Start.Y}; End: {End.X}:{End.Y}";

    public Point Start { get; set; }
    public Point End { get; set; }

    public Line(Point start, Point end) => (Start, End) = (start, end);

    public void Move(double deltaX, double deltaY)
    {
        Start.Move(deltaX, deltaY);
        End.Move(deltaX, deltaY);
    }

    public void Resize(double xScale, double yScale, Point centralPoint)
    {
        double xDelta = (End.X - Start.X) * xScale;
        double yDelta = (End.Y - Start.Y) * yScale;

        double xStartDelta = (Start.X - centralPoint.X) * xScale;
        double yStartDelta = (Start.Y - centralPoint.Y) * yScale;

        Start = new Point(centralPoint.X + xStartDelta, centralPoint.Y + yStartDelta);
        End = new Point(Start.X + xDelta, Start.Y + yDelta);
    }

    public bool Equals(Line? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Line)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Start, End);
}

public class Element
{
    public override string ToString()
    {
        return $"{string.Join("; ", Lines.Select(x => x))}";
    }

    public ElementType ElementType { get; }
    public List<Line> Lines { get; set; } = new();
    public Point StartPosition { get; set; } = Point.None;
    public Point EndPosition { get; set; } = Point.None;

    public Element(ElementType elementType) => ElementType = elementType;

    public Element(ElementType elementType, Line line) : this(elementType)
    {
        Lines.Add(line);
        StartPosition = new Point(Math.Min(line.Start.X, line.End.X), Math.Max(line.Start.Y, line.End.Y));
        EndPosition = new Point(Math.Max(line.Start.X, line.End.X), Math.Min(line.Start.Y, line.End.Y));
    }

    public Element(ElementType elementType, List<Line> lines, Point startPosition, Point endPosition) : this(elementType)
    {
        Lines.AddRange(lines);
        StartPosition = startPosition;
        EndPosition = endPosition;
    }

    public double Length() => Math.Abs(EndPosition.X - StartPosition.X);

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

    public bool IsSameTypeWith(ElementType compared) => ElementType.IsSame(compared);
}