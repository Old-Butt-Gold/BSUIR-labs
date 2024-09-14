using System.IO;
using System.Windows;
using ChessLogic.Moves;
using ChessLogic.Pieces;

namespace ChessLogic.Bot;

public class BotManager : IDisposable
{
    static readonly string PathToBot = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "stockfish.exe";

    readonly StockfishManager? _stockfishManager;

    public bool IsCreatedBot { get; set; }

    public (Move? move, PieceType? pieceType, IEnumerable<Move>? moves) GetBestMove(GameManager gameManager)
    {
        return _stockfishManager!.GetMoveByPosition(gameManager.StateString, gameManager.LegalMovesForPieces);
    }

    public BotManager(BotDifficulty botDifficulty)
    {
        if (File.Exists(PathToBot))
        {
            IsCreatedBot = true;
            _stockfishManager = new StockfishManager(PathToBot, botDifficulty);
        }
        else
        {
            IsCreatedBot = false;
            MessageBox.Show("stockfish.exe wasn't found in current directory");
        }
    }

    public void Dispose()
    {
        _stockfishManager?.Dispose();
    }
}