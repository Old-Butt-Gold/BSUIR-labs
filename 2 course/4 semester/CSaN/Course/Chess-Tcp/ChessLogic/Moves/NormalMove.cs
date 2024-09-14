using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public class NormalMove : Move
{
    public override MoveType Type => MoveType.Normal;
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    public NormalMove(Position fromPos, Position toPos) => (FromPos, ToPos) = (fromPos, toPos);
    
    public override bool Execute(Board board)
    {
        var piece = board[FromPos];
        bool capture = !board.IsEmpty(ToPos);
        board[ToPos] = piece;
        board[FromPos] = null;
        piece.HasMoved = true;

        return capture || piece.Type == PieceType.Pawn;
    }
}