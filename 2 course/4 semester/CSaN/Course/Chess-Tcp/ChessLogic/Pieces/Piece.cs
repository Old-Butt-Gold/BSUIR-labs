using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public abstract class Piece
{
    public abstract PieceType Type { get; }

    public abstract Player Color { get; }
    
    public bool HasMoved { get; set; }

    public abstract Piece Copy();

    public abstract IEnumerable<Move> GetMoves(Position from, Board board);
    
    protected IEnumerable<Position> MovePositionsInDirections(Position from, Board board, IEnumerable<Direction> directions)
    {
        //объединяет последовательности в одну
        return directions.SelectMany(MovePositionsInDirection); 
        
        IEnumerable<Position> MovePositionsInDirection(Direction direction)
        {
            for (var pos = from + direction; Board.IsInside(pos); pos += direction)
            {
                if (board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }
            
                yield break;
            }
        }
    }

    public virtual bool CanCaptureOpponentKing(Position from, Board board)
    {
        return GetMoves(from, board).Any(move => board[move.ToPos] is { Type: PieceType.King }); 
    }
}