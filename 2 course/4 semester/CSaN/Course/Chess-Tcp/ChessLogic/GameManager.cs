using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;
using ChessLogic.Pieces;
using ChessLogic.ResultReasons;

namespace ChessLogic;

public class GameManager
{
    public Board Board { get; }
    public Player CurrentPlayer { get; private set; }
    public Result? Result { get; private set; }

    int _noCaptureOrPawnMoves;
    public string StateString { get; private set; }

    readonly Dictionary<string, int> _stateHistory = new();

    public GameManager(Player player)
    {
        CurrentPlayer = player;
        Board = Board.Initial();

        StateString = new StateString(CurrentPlayer, Board).ToString();
        _stateHistory[StateString] = 1;
    }

    public void MakeMove(Move move)
    {
        Board.SetPawnSkipPosition(CurrentPlayer, null);
        bool captureOrPawn = move.Execute(Board);

        if (captureOrPawn)
        {
            _noCaptureOrPawnMoves = 0;
            _stateHistory.Clear();
        }
        else
        {
            _noCaptureOrPawnMoves++;
        }

        CurrentPlayer = CurrentPlayer.Opponent();
        UpdateStateString();
        CheckForGameOver();
    }

    public IEnumerable<Move> LegalMovesForPieces(Position position)
    {
        if (Board.IsEmpty(position) || Board[position].Color != CurrentPlayer)
        {
            return Enumerable.Empty<Move>();
        }
        
        Piece piece = Board[position];
        IEnumerable<Move> moveCandidates = piece.GetMoves(position, Board);
        return moveCandidates.Where(move => move.IsLegal(Board));
    }

    IEnumerable<Move> AllLegalMovesFor(Player player)
    {
        var moveCandidates = Board.PiecePositionsFor(player).SelectMany(position => Board[position].GetMoves(position, Board));

        return moveCandidates.Where(move => move.IsLegal(Board));
    }

    void CheckForGameOver()
    {
        if (!AllLegalMovesFor(CurrentPlayer).Any())
        {
            Result = Board.IsInCheck(CurrentPlayer) 
                ? new(CurrentPlayer.Opponent(), EndReason.Checkmate) 
                : new(Player.None, EndReason.Stalemate);
        }

        if (Board.InsufficientMaterial())
        {
            Result = new Result(Player.None, EndReason.InsufficientMaterial);
        }

        if (FiftyMoveRule())
        {
            Result = new(Player.None, EndReason.FiftyMoveRule);
        }

        if (ThreefoldRepetition())
        {
            Result = new Result(Player.None, EndReason.ThreefoldRepetition);
        }
    }

    public bool IsGameOver() => Result != null;

    bool FiftyMoveRule() => _noCaptureOrPawnMoves / 2 == 50; //по 50 на одного игрока

    void UpdateStateString()
    {
        StateString = new StateString(CurrentPlayer, Board).ToString();
        
        if (!_stateHistory.TryAdd(StateString, 1))
        {
            _stateHistory[StateString]++;
        }
    }

    bool ThreefoldRepetition() => _stateHistory[StateString] is 3;
    
    
}