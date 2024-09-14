using ChessLogic.CoordinateClasses;

namespace ChessLogic.Moves;

public class EnPassantMove : Move
{
    public override MoveType Type => MoveType.EnPassant;
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    readonly Position _capturePosition;

    public EnPassantMove(Position fromPos, Position toPos)
    {
        FromPos = fromPos;
        ToPos = toPos;

        _capturePosition = new Position(fromPos.Row, toPos.Column);
    }
    
    public override bool Execute(Board board)
    {
        new NormalMove(FromPos, ToPos).Execute(board);
        board[_capturePosition] = null;

        return true;
    }
}