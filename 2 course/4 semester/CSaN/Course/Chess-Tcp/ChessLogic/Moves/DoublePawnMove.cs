using ChessLogic.CoordinateClasses;

namespace ChessLogic.Moves;

public class DoublePawnMove : Move
{
    public override MoveType Type => MoveType.DoublePawn;
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    readonly Position _skippedPosition;

    public DoublePawnMove(Position fromPos, Position toPos)
    {
        (FromPos, ToPos) = (fromPos, toPos);
        _skippedPosition = new Position((fromPos.Row + toPos.Row) / 2, fromPos.Column);
    }

    public override bool Execute(Board board)
    {
        Player player = board[FromPos].Color;
        board.SetPawnSkipPosition(player, _skippedPosition);

        new NormalMove(FromPos, ToPos).Execute(board);

        return true;
    }
}