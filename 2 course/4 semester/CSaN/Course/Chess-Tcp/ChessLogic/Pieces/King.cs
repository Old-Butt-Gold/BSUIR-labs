using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;

namespace ChessLogic.Pieces;

public class King : Piece
{
    public override PieceType Type => PieceType.King;
    public override Player Color { get; }
    
    static readonly Direction[] Directions = {
        Direction.North,
        Direction.East,
        Direction.South,
        Direction.West,
        Direction.NorthEast,
        Direction.NorthWest,
        Direction.SouthEast,
        Direction.SouthWest,
    };

    public King(Player color) => Color = color;

    bool IsUnmovedRook(Position pos, Board board)
    {
        if (board.IsEmpty(pos))
        {
            return false;
        }

        return board[pos] is { Type: PieceType.Rook, HasMoved: false };
    }

    bool AllEmpty(IEnumerable<Position> positions, Board board) => positions.All(board.IsEmpty);

    bool CanCastleKingSide(Position from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }

        Position rookPosition;
        Position[] betweenPositions;
        
        if (!Board.IsBoardReversed)
        {
            rookPosition = new Position(from.Row, 7);
            betweenPositions = new Position[]{ new(from.Row, 5), new(from.Row, 6) };
        }
        else
        {
            rookPosition = new Position(from.Row, 0);
            betweenPositions = new Position[]{ new(from.Row, 1), new(from.Row, 2) };
        }

        return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
    }

    bool CanCastleQueenSide(Position from, Board board)
    {
        if (HasMoved)
        {
            return false;
        }
        
        Position rookPosition;
        Position[] betweenPositions;
        
        if (!Board.IsBoardReversed)
        {
            rookPosition = new Position(from.Row, 0);
            betweenPositions = new Position[]{ new(from.Row, 1), new(from.Row, 2), new(from.Row, 3) };
        }
        else
        {
            rookPosition = new Position(from.Row, 7);
            betweenPositions = new Position[]{ new(from.Row, 4), new(from.Row, 5), new(from.Row, 6) };
        }
        
        return IsUnmovedRook(rookPosition, board) && AllEmpty(betweenPositions, board);
    }
    
    public override Piece Copy() => new King(Color)
    {
        HasMoved = HasMoved
    };

    public override IEnumerable<Move> GetMoves(Position from, Board board)
    {
        foreach (var to in MovePositions())
        {
            yield return new NormalMove(from, to);
        }

        if (CanCastleKingSide(from, board))
        {
            yield return new CastleMove(MoveType.CastleKS, from);
        }

        if (CanCastleQueenSide(from, board))
        {
            yield return new CastleMove(MoveType.CastleQS, from);
        }
        
        IEnumerable<Position> MovePositions()
        {
            foreach (var direction in Directions)
            {
                var to = from + direction;

                if (Board.IsInside(to) && (board.IsEmpty(to) || board[to].Color != Color))
                {
                    yield return to;
                }
            }
        }
    }
}