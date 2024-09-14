using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessLogic.Moves;

public class PawnPromotion : Move
{
    public override MoveType Type => MoveType.PawnPromotion;
    public override Position FromPos { get; }
    public override Position ToPos { get; }

    public PawnPromotion(Position fromPos, Position toPos, PieceType newType) => (FromPos, ToPos, _newType) = (fromPos, toPos, newType);

    readonly PieceType _newType;
    
    public override bool Execute(Board board)
    {
        Piece pawn = board[FromPos];
        board[FromPos] = null;

        Piece promotionPiece = CreatePromotionPiece(pawn.Color);
        promotionPiece.HasMoved = true;
        board[ToPos] = promotionPiece;

        return true;
        
        Piece CreatePromotionPiece(Player color) =>
            _newType switch
            {
                PieceType.Knight => new Knight(color),
                PieceType.Bishop => new Bishop(color),
                PieceType.Rook => new Rook(color),
                _ => new Queen(color),
            };
    }
}