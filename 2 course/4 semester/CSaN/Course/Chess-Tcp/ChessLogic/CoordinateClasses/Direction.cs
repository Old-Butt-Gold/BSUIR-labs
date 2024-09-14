namespace ChessLogic.CoordinateClasses;

public class Direction
{
    public static readonly Direction North = new(-1, 0);
    public static readonly Direction South = new(1, 0);
    public static readonly Direction West = new(0, -1);
    public static readonly Direction East = new(0, 1);
    public static readonly Direction NorthEast = North + East;
    public static readonly Direction NorthWest = North + West;
    public static readonly Direction SouthEast = South + East;
    public static readonly Direction SouthWest = South + West;
    
    public int RowDelta { get; }
    public int ColumnDelta { get; }

    public Direction(int rowDelta, int columnDelta) => (RowDelta, ColumnDelta) = (rowDelta, columnDelta);

    public static Direction operator +(Direction direction1, Direction direction2)
    {
        return new Direction(direction1.RowDelta + direction2.RowDelta,
            direction1.ColumnDelta + direction2.ColumnDelta);
    }

    public static Direction operator *(int scalar, Direction direction)
    {
        return new(scalar * direction.RowDelta, scalar * direction.ColumnDelta);
    }
}