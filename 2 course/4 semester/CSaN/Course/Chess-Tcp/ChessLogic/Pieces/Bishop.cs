using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class Bishop : Piece
{
    public override PieceType Type => PieceType.Bishop;
    public override Player Color { get; }

    static readonly Direction[] Directions = {
        Direction.NorthWest,
        Direction.NorthEast,
        Direction.SouthEast,
        Direction.SouthWest,
    };

    public Bishop(Player color) => Color = color;

    public override Piece Copy() => new Bishop(Color)
    {
        HasMoved = HasMoved
    };

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositionsInDirections(from, board, Directions).Select(to => new NormalMove(from, to));
    }
}