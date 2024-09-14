namespace ChessLogic.CoordinateClasses;

public class Position : IEquatable<Position>
{
    public int Row { get; }
    public int Column { get; }

    public Position(int row, int column) => (Row, Column) = (row, column);

    public Player SquareColor() => Row + Column % 2 is 0 ? Player.White : Player.Black;

    public override string ToString() => $"{Row}{(char) (Column + 'a')}";

    public bool Equals(Position? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Row == other.Row && Column == other.Column;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Position)obj);
    }

    public override int GetHashCode() => HashCode.Combine(Row, Column);

    public static bool operator ==(Position left, Position right) => EqualityComparer<Position>.Default.Equals(left, right);

    public static bool operator !=(Position left, Position right) => !(left == right);

    public static Position operator +(Position position, Direction direction)
    {
        return new(position.Row + direction.RowDelta, position.Column + direction.ColumnDelta);
    }
}