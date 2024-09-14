using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using ChessLogic;
using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;
using ChessLogic.Pieces;

namespace ChessUI;

public class BoardDrawer
{
    public Position? SelectedPosition { get; private set; }
    Position? _kingPosition;
    
    readonly Image[,] _pieceImages = new Image[8, 8];
    
    readonly Shape[,] _highLights = new Shape[8, 8];

    readonly Dictionary<Position, Move> _movesCache = new();

    public Brush DangerBrush { get; set; } = new SolidColorBrush(Color.FromArgb(150, 245, 39, 65));
    public Brush LegalBrush { get; set; } = new SolidColorBrush(Color.FromArgb(150, 125, 255, 125));
    
    public void DrawBoard(Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _pieceImages[i, j].Source = Images.GetImage(board[i, j]);
            }
        }
    }

    public void ClearPieceImages(UniformGrid pieceGrid)
    {
        pieceGrid.Children.Clear();
    }

    public void InitializePieceImages(UniformGrid pieceGrid)
    {
        ClearPieceImages(pieceGrid);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Image image = new Image();
                _pieceImages[i, j] = image;
                
                pieceGrid.Children.Add(image);
            }
        }
    }

    public void MoveImagePiece(Position from, Position to, Player player, PieceType pieceType)
    {
        _pieceImages[to.Row, to.Column].Source = Images.GetImage(player, pieceType);
        _pieceImages[from.Row, from.Column].Source = null;
    }

    public void ClearHighLights(UniformGrid highLightGrid)
    {
        highLightGrid.Children.Clear();   
    }

    public void InitializeHighLights(UniformGrid highLightGrid)
    {
        ClearHighLights(highLightGrid);
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Ellipse ellipse = new Ellipse
                {
                    Height = 45,
                    Width = 45
                };

                _highLights[i, j] = ellipse;
                highLightGrid.Children.Add(ellipse);
            }
        }
    }

    public void ReloadBoard(Board board)
    {
        SelectedPosition = null;
        _kingPosition = null;
        DrawBoard(board);
        _movesCache.Clear();
        foreach (var highLight in _highLights)
        {
            highLight.Fill = Brushes.Transparent;
        }
    }
    
    public void DrawKingCheck(Board board, Player currentPlayer)
    {
        if (board.IsInCheck(currentPlayer))
        {
            _kingPosition = board.PiecePositionsFor(currentPlayer)
                .First(x => board[x].Type == PieceType.King);
            Draw(_kingPosition, DangerBrush);
        }
        else
        {
            Draw(_kingPosition, Brushes.Transparent);
            _kingPosition = null;
        }

        void Draw(Position kingPos, Brush brush)
        {
            if (kingPos != null)
            {
                _highLights[kingPos.Row, kingPos.Column].Fill = brush;
            }
        }
    }

    public void ShowPossibleMoves(Position position, IEnumerable<Move> legalMovesForPieces)
    {
        if (legalMovesForPieces.Any())
        {
            SelectedPosition = position;
            
            _movesCache.Clear();
            foreach (var move in legalMovesForPieces)
            {
                _movesCache[move.ToPos] = move;
            }
            _movesCache[SelectedPosition] = null!;
            
            foreach (var to in _movesCache.Keys)
            {
                _highLights[to.Row, to.Column].Fill = LegalBrush;
            }
        }
    }
    
    public Move? TryToGetMove(Position position)
    {
        SelectedPosition = null;
        
        foreach (var to in _movesCache.Keys)
        {
            _highLights[to.Row, to.Column].Fill = Brushes.Transparent;
        }
        
        return _movesCache.GetValueOrDefault(position);
    }
}