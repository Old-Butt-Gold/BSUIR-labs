using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class Knight : Piece
{
    public override PieceType Type => PieceType.Knight;
    public override Player Color { get; }

    public Knight(Player color) => Color = color;

    public override Piece Copy() => new Knight(Color)
    {
        HasMoved = HasMoved
    };

    IEnumerable<Position> MovePositions(Position from, Board board)
    {
        return PotentialToPositions()
            .Where(pos => Board.IsInside(pos) && (board.IsEmpty(pos) || board[pos].Color != Color));
        
        IEnumerable<Position> PotentialToPositions()
        {
            foreach (var verticalDir in new[] { Direction.North, Direction.South })
            {
                foreach (var horizontalDir in new[] { Direction.West, Direction.East })
                {
                    yield return from + 2 * verticalDir + horizontalDir;
                    yield return from + 2 * horizontalDir + verticalDir;
                }
            }
        }
    }

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        return MovePositions(from, board).Select(to => new NormalMove(from, to));
    }
}