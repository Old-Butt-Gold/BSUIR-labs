using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using ChessLogic.CoordinateClasses;

namespace ChessUI;

public class UIChessManager : INotifyPropertyChanged
{
    private Grid _boardGrid;
    private UniformGrid _highlightGrid;
    private UniformGrid _pieceGrid;
    private ContentControl _menuContainer;

    public void SetCursor(Player player)
    {
        BoardGrid.Cursor = player == Player.White ? ChessCursors.WhiteCursor : ChessCursors.BlackCursor;
    }
    
    public bool IsMenuOnScreen() => MenuContainer.Content != null;
    
    public Grid BoardGrid
    {
        get => _boardGrid;
        set
        {
            if (_boardGrid != value)
            {
                _boardGrid = value;
                OnPropertyChanged(nameof(BoardGrid));
            }
        }
    }
    
    public UniformGrid HighLightGrid
    {
        get => _highlightGrid;
        set
        {
            if (_highlightGrid != value)
            {
                _highlightGrid = value;
                OnPropertyChanged(nameof(HighLightGrid));
            }
        }
    }

    public UniformGrid PieceGrid
    {
        get => _pieceGrid;
        set
        {
            if (_pieceGrid != value)
            {
                _pieceGrid = value;
                OnPropertyChanged(nameof(PieceGrid));
            }
        }
    }
    
    public ContentControl MenuContainer
    {
        get => _menuContainer;
        set
        {
            if (_menuContainer != value)
            {
                _menuContainer = value;
                OnPropertyChanged(nameof(MenuContainer));
            }
        }
    }
    
    public Position ToSquarePosition(MouseButtonEventArgs e)
    {
        Point point = e.GetPosition(PieceGrid);
        double rowSize = PieceGrid.ActualWidth / 8;
        double colSize = PieceGrid.ActualHeight / 8;
        int row = (int)(point.Y / colSize);
        int column = (int)(point.X / rowSize);
        return new Position(row, column);
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}