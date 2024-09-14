using System.Text;
using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessLogic;

public class StateString
{
    readonly StringBuilder _sb = new();

    public StateString(Player currentPlayer, Board board) //FEN
    {
        AddPiecePlacement(board);
        _sb.Append(' ');
        AddCurrentPlayer(currentPlayer);
        _sb.Append(' ');
        AddCastlingRights(board);
        _sb.Append(' ');
        AddEnPassant(board, currentPlayer);
    }

    public override string ToString() => _sb.ToString();

    void AddPiecePlacement(Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            if (i != 0)
            {
                _sb.Append('/');
            }
            
            AddRowData(board, i);
        }
        
        void AddRowData(Board board, int row)
        {
            int empty = 0;

            for (int i = 0; i < 8; i++)
            {
                if (board[row, i] is null)
                {
                    empty++;
                }
                else
                {

                    if (empty > 0)
                    {
                        _sb.Append(empty);
                        empty = 0;
                    }

                    _sb.Append(PieceChar(board[row, i]));
                }
            }

            if (empty > 0)
            {
                _sb.Append(empty);
            }
        
            char PieceChar(Piece piece)
            {
                char c = piece.Type switch
                {
                    PieceType.Pawn => 'p',
                    PieceType.Knight => 'n',
                    PieceType.Rook => 'r',
                    PieceType.Bishop => 'b',
                    PieceType.Queen => 'q',
                    PieceType.King => 'k',
                    _ => ' ',
                };

                return piece.Color is Player.White ? char.ToUpper(c) : c;
            }
        }
    }

    void AddCurrentPlayer(Player currentPlayer) => _sb.Append(currentPlayer == Player.White ? 'w' : 'b');

    void AddCastlingRights(Board board)
    {
        bool castleWks = board.CastleRightKs(Player.White);
        bool castleBks = board.CastleRightKs(Player.Black);
        bool castleWqs = board.CastleRightQs(Player.White);
        bool castleBqs = board.CastleRightQs(Player.Black);

        if (!(castleWqs || castleWks || castleBqs || castleBks))
        {
            _sb.Append('-');
            return;
        }

        if (castleWks)
        {
            _sb.Append('K');
        }

        if (castleWqs)
        {
            _sb.Append('Q');
        }

        if (castleBks)
        {
            _sb.Append('k');
        }

        if (castleBqs)
        {
            _sb.Append('q');
        }
    }

    void AddEnPassant(Board board, Player currentPlayer)
    {
        if (!board.CanCaptureEnPassant(currentPlayer))
        {
            _sb.Append('-');
            return;
        }

        Position position = board.GetPawnSkipPosition(currentPlayer.Opponent());
        char file = (char)('a' + position.Column);
        int rank = 8 - position.Row;

        _sb.Append(file);
        _sb.Append(rank);
    }
}