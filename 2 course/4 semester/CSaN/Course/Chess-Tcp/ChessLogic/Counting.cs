using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessLogic;

public class Counting
{
    readonly Dictionary<PieceType, int> WhiteCount = new();
    readonly Dictionary<PieceType, int> BlackCount = new();
    
    public int TotalCount { get; private set; }

    public Counting()
    {
        foreach (var type in Enum.GetValues<PieceType>())
        {
            WhiteCount[type] = 0;
            BlackCount[type] = 0;
        }
    }

    public void Increment(Player color, PieceType type)
    {
        if (color == Player.White)
        {
            WhiteCount[type]++;
        } else if (color == Player.Black)
        {
            BlackCount[type]++;
        }

        TotalCount++;
    }

    public int White(PieceType type) => WhiteCount[type];
    public int TotalWhite() => WhiteCount.Values.Sum();
    public int Black(PieceType type) => BlackCount[type];
    public int TotalBlack() => BlackCount.Values.Sum();
}