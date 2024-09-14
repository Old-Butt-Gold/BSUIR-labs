using ChessLogic.CoordinateClasses;

namespace ChessLogic.ResultReasons;

public class Result
{
    public Player Winner { get; }
    public EndReason Reason { get; }

    public Result(Player winner, EndReason reason) => (Winner, Reason) = (winner, reason);
}