using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;
using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessUI;

public static class Images
{
    static ImageSource LoadImage(string filePath) => new BitmapImage(new Uri(filePath, UriKind.Relative));

    static readonly Dictionary<PieceType, ImageSource> WhiteSources = new()
    {
        { PieceType.Pawn, LoadImage("./Assets/PawnW.png")},
        { PieceType.Bishop, LoadImage("./Assets/BishopW.png")},
        { PieceType.Knight, LoadImage("./Assets/KnightW.png")},
        { PieceType.Rook, LoadImage("./Assets/RookW.png")},
        { PieceType.Queen, LoadImage("./Assets/QueenW.png")},
        { PieceType.King, LoadImage("./Assets/KingW.png")},
    };
    
    static readonly Dictionary<PieceType, ImageSource> BlackSources = new()
    {
        { PieceType.Pawn, LoadImage("./Assets/PawnB.png")},
        { PieceType.Bishop, LoadImage("./Assets/BishopB.png")},
        { PieceType.Knight, LoadImage("./Assets/KnightB.png")},
        { PieceType.Rook, LoadImage("./Assets/RookB.png")},
        { PieceType.Queen, LoadImage("./Assets/QueenB.png")},
        { PieceType.King, LoadImage("./Assets/KingB.png")},
    };

    public static ImageSource GetImage(Player color, PieceType type)
    {
        return (color switch
        {
            Player.White => WhiteSources[type],
            Player.Black => BlackSources[type],
            _ => null,
        })!;
    }

    public static ImageSource GetImage(Piece piece) 
        => piece is null ? null : GetImage(piece.Color, piece.Type);
}