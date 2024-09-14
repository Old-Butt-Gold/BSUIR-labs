using System.Windows;
using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessUI.ContentControls;

public partial class PromotionMenu
{
    public event Action<PieceType>? PieceSelected;
    
    public PromotionMenu(Player player)
    {
        InitializeComponent();
        ImgQueen.Source = Images.GetImage(player, PieceType.Queen);
        ImgBishop.Source = Images.GetImage(player, PieceType.Bishop);
        ImgRook.Source = Images.GetImage(player, PieceType.Rook);
        ImgKnight.Source = Images.GetImage(player, PieceType.Knight);
    }

    private void Queen_Click(object sender, RoutedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Queen);
    }

    private void Bishop_Click(object sender, RoutedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Bishop);
    }

    private void Rook_Click(object sender, RoutedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Rook);
    }

    private void Knight_Click(object sender, RoutedEventArgs e)
    {
        PieceSelected?.Invoke(PieceType.Knight);
    }

}