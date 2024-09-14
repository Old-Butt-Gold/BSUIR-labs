using System.Windows;
using ChessLogic;
using ChessLogic.CoordinateClasses;
using ChessLogic.ResultReasons;

namespace ChessUI.ContentControls;

public partial class GameOverMenu
{
    public event Action<Option>? OptionSelected;
    
    public GameOverMenu(GameManager gameManager)
    {
        InitializeComponent();

        var result = gameManager.Result;

        WinnerText.Text = GetWinnerText(result!.Winner);
        ReasonText.Text = GetReasonText(result.Reason, gameManager.CurrentPlayer);
    }
    
    static string GetWinnerText(Player winner) =>
        winner switch
        {
            Player.White => "WHITE WINS",
            Player.Black => "BLACK WINS",
            _ => "DRAW",
        };

    static string PlayerString(Player player) =>
        player switch
        {
            Player.White => "WHITE",
            Player.Black => "BLACK",
            _ => string.Empty,
        };
    
    static string GetReasonText(EndReason reason, Player currentPlayer)
    {
        return reason switch
        {
            EndReason.Stalemate => $"STALEMATE – {PlayerString(currentPlayer)} CAN'T MOVE",
            EndReason.Checkmate => $"CHECKMATE – {PlayerString(currentPlayer)} CAN'T MOVE",
            EndReason.FiftyMoveRule => $"FIFTY-MOVE RULE",
            EndReason.InsufficientMaterial => $"INSUFFICIENT MATERIAL",
            EndReason.ThreefoldRepetition => $"THREEFOLD REPETITION",
            _ => string.Empty,
        };
    }
    
    private void Exit_OnClick(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Exit);
    }

    private void Restart_OnClick(object sender, RoutedEventArgs e)
    {
        OptionSelected?.Invoke(Option.Restart);
    }
}