using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class Queen : Piece
{
    public override PieceType Type => PieceType.Queen;
    public override Player Color { get; }
    
    static readonly Direction[] Directions = {
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West,
        Direction.NorthEast,
        Direction.NorthWest,
        Direction.SouthEast,
        Direction.SouthWest,
    };

    public Queen(Player color) => Color = color;

    public override Piece Copy() => new Queen(Color)
    {
        HasMoved = HasMoved
    };

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositionsInDirections(from, board, Directions).Select(to => new NormalMove(from, to));
    }
}