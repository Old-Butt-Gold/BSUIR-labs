namespace ChessLogic.Moves;

public enum MoveType
{
    Normal,
    CastleKS, //KingSide
    CastleQS, //QueenSide
    DoublePawn,
    EnPassant,
    PawnPromotion,
}