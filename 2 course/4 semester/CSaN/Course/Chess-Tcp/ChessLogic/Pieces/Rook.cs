using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class Rook : Piece
{
    public override PieceType Type => PieceType.Rook;
    public override Player Color { get; }
    
    static readonly Direction[] Directions = {
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West,
    };

    public Rook(Player color) => Color = color;

    public override Piece Copy() => new Rook(Color)
    {
        HasMoved = HasMoved
    };

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositionsInDirections(from, board, Directions).Select(to => new NormalMove(from, to));
    }
}