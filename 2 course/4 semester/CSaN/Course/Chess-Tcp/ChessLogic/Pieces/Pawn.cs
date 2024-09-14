using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class Pawn : Piece
{
    public override PieceType Type => PieceType.Pawn;
    public override Player Color { get; }

    readonly Direction Forward;

    public Pawn(Player color)
    {
        Color = color;
        switch (color)
        {
            case Player.White when !Board.IsBoardReversed:
                Forward = Direction.North;
                break;
            case Player.White when Board.IsBoardReversed:
                Forward = Direction.South;
                break;
            case Player.Black when !Board.IsBoardReversed:
                Forward = Direction.South;
                break;
            case Player.Black when Board.IsBoardReversed:
                Forward = Direction.North;
                break;
        }
    }

    public override Piece Copy() => new Pawn(Color)
    {
        HasMoved = HasMoved
    };

    static bool CanMoveTo(Position pos, Board board) => Board.IsInside(pos) && board.IsEmpty(pos);

    bool CanCaptureAt(Position pos, Board board)
    {
        if (!Board.IsInside(pos) || board.IsEmpty(pos))
        {
            return false;
        }

        return board[pos].Color != Color;
    }

    IEnumerable<Move> PromotionMoves(Position from, Position to)
    {
        yield return new PawnPromotion(from, to, PieceType.Knight);
        yield return new PawnPromotion(from, to, PieceType.Bishop);
        yield return new PawnPromotion(from, to, PieceType.Rook);
        yield return new PawnPromotion(from, to, PieceType.Queen);
    }
    
    IEnumerable<Move> ForwardMoves(Position from, Board board)
    {
        Position oneMovePos = from + Forward;

        if (CanMoveTo(oneMovePos, board))
        {
            if (oneMovePos.Row is 0 or 7)
            {
                foreach (Move promMove in PromotionMoves(from, oneMovePos))
                {
                    yield return promMove;
                }
            }
            else
            {
                yield return new NormalMove(from, oneMovePos);
            }

            Position twoMovesPos = oneMovePos + Forward;

            if (!HasMoved && CanMoveTo(twoMovesPos, board))
            {
                yield return new DoublePawnMove(from, twoMovesPos);
            }
        }
    }

    IEnumerable<Move> DiagonalMoves(Position from, Board board)
    {
        foreach (var direction in new[]{Direction.West, Direction.East})
        {
            Position to = from + Forward + direction; //Уже задано North или South в Forward

            if (to == board.GetPawnSkipPosition(Color.Opponent()))
            {
                yield return new EnPassantMove(from, to);
            }
            else if (CanCaptureAt(to, board))
            {
                if (to.Row is 0 or 7)
                {
                    foreach (Move promMove in PromotionMoves(from, to))
                    {
                        yield return promMove;
                    }
                }
                else
                {
                    yield return new NormalMove(from, to);
                }
            }
        }
    }
    
    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return ForwardMoves(from, board).Concat(DiagonalMoves(from, board));
    }

    public override bool CanCaptureOpponentKing(Position from, Board board)
    {
        return DiagonalMoves(from, board).Any(move => board[move.ToPos] is { Type: PieceType.King }); 
        //can hit only by diagonal
    }
}