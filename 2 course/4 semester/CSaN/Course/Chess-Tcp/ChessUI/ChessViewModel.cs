using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using ChessClient;
using ChessLogic;
using ChessLogic.Bot;
using ChessLogic.CoordinateClasses;
using ChessLogic.Moves;
using ChessLogic.Pieces;
using ChessUI.Commands;
using ChessUI.ContentControls;
using ProcessStartInfo = System.Diagnostics.ProcessStartInfo;

namespace ChessUI;

public class ChessViewModel : IDisposable
{
    public ICommand? MouseDownCommand { get; private set; }
    public ICommand ShowHelpCommand { get; private set; } = new RelayCommand(ShowHelp);

    static void ShowHelp(object obj)
    {
        string url = "https://www.chess.com/ru/kak-igrat-v-shakhmaty";
        var info = new ProcessStartInfo(url)
        {
            UseShellExecute = true
        };
        Process.Start(info);
    }

    public UIChessManager UiChessManager { get; private set; } = new();
    public BoardDrawer BoardDrawer { get; private set; } = new();
    public GameManager GameManager { get; private set; }
    public BotManager? BotManager { get; private set; }
    
    public Client ChessClient { get; set; }

    bool IsBotThinking { get; set; }
    bool IsEnemyThinking { get; set; }
    Player StartPlayer { get; set; }

    public async void StartPvp(Player startPlayer)
    {
        Stop();
        
        IsEnemyThinking = false;
        IsPromotion = false;
        
        StartPlayer = startPlayer;

        Board.IsBoardReversed = StartPlayer == Player.Black;

        MouseDownCommand = new RelayCommand(MouseDownPvp, _ => !UiChessManager.IsMenuOnScreen() && !IsEnemyThinking);

        Reload();
        
        if (startPlayer == Player.Black)
        {
            await GetMoveFromOpponent();
        }
    }

    public void StartBot(Player startPlayer, BotDifficulty botDifficulty)
    {
        Stop();

        Board.IsBoardReversed = false;

        StartPlayer = startPlayer;

        CancellationTokenSource = new CancellationTokenSource();
        CancellationToken = CancellationTokenSource.Token;
        BotManager = new(botDifficulty);

        MouseDownCommand = new RelayCommand(MouseDownPlayerVBot,
            _ => !UiChessManager.IsMenuOnScreen() && !IsBotThinking);
        Reload();
    }

    void Reload()
    {
        BoardDrawer.InitializeHighLights(UiChessManager.HighLightGrid);
        BoardDrawer.InitializePieceImages(UiChessManager.PieceGrid);
        GameManager = new GameManager(Player.White);
        BoardDrawer.ReloadBoard(GameManager.Board);
        UiChessManager.SetCursor(GameManager.CurrentPlayer);
    }

    public void Stop() //Для остановки и смены режима игры
    {
        try
        {
            CancellationTokenSource?.Cancel();
            CancellationTokenSource?.Dispose();
        }
        catch { }

        BoardDrawer.ClearHighLights(UiChessManager.HighLightGrid);
        BoardDrawer.ClearPieceImages(UiChessManager.PieceGrid);
    }

    void HandlePromotionMove(Position from, Position to)
    {
        BoardDrawer.MoveImagePiece(from, to, GameManager.CurrentPlayer, PieceType.Pawn);

        PromotionMenu promotionMenu = new PromotionMenu(GameManager.CurrentPlayer);
        UiChessManager.MenuContainer.Content = promotionMenu;

        promotionMenu.PieceSelected += async type =>
        {
            UiChessManager.MenuContainer.Content = null;
            Move move = new PawnPromotion(from, to, type);
            HandleMove(move);
            
            if (IsPromotion)
            {
                await ChessClient.SendMessageAsync($"{ClientAction.MakeMove}{Client.MessageRegex}{move.FromPos}:{move.ToPos}:{StartPlayer}:{type}");
            }
        };
    }

    void HandleMove(Move move)
    {
        GameManager.MakeMove(move);
        
        BoardDrawer.DrawBoard(GameManager.Board);
        UiChessManager.SetCursor(GameManager.CurrentPlayer);
        
        if (GameManager.IsGameOver())
        {
            GameOverMenu gameOverMenu = new GameOverMenu(GameManager);
            UiChessManager.MenuContainer.Content = gameOverMenu;
            gameOverMenu.OptionSelected += option =>
            {
                if (option == Option.Restart)
                {
                    Reload();
                    UiChessManager.MenuContainer.Content = null;
                }
                else
                {
                    Application.Current.Shutdown();
                }
            };
        }
    }

    #region Bots game type

    CancellationTokenSource? CancellationTokenSource { get; set; }
    CancellationToken CancellationToken { get; set; }
    
    async void MouseDownPlayerVBot(object obj)
    {
        if (obj is MouseButtonEventArgs e)
        {
            if (StartPlayer == GameManager.CurrentPlayer || !BotManager!.IsCreatedBot)
            {
                var position = UiChessManager.ToSquarePosition(e);

                if (BoardDrawer.SelectedPosition is null)
                {
                    IEnumerable<Move> moves = GameManager.LegalMovesForPieces(position);
                    BoardDrawer.ShowPossibleMoves(position, moves);
                }
                else
                {
                    var move = BoardDrawer.TryToGetMove(position);
                    if (move != null)
                    {
                        if (move.Type == MoveType.PawnPromotion)
                        {
                            HandlePromotionMove(move.FromPos, move.ToPos);
                        }
                        else
                        {
                            HandleMove(move);
                        }
                    }
                }

                BoardDrawer.DrawKingCheck(GameManager.Board, GameManager.CurrentPlayer);
            }
            
            if (GameManager.CurrentPlayer == StartPlayer.Opponent() && BotManager!.IsCreatedBot)
            {
                IsBotThinking = true;
                await HandleBotMoveAsync(CancellationToken);
                IsBotThinking = false;
            }
        }
    }

    async Task HandleBotMoveAsync(CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(750, cancellationToken);

            var (moveFinal, pieceType, moves) = BotManager!.GetBestMove(GameManager);
            if (moveFinal != null)
            {
                BoardDrawer.ShowPossibleMoves(moveFinal.FromPos, moves!);

                await Task.Delay(750, cancellationToken);

                if (moveFinal.Type == MoveType.PawnPromotion)
                {
                    moveFinal = new PawnPromotion(moveFinal.FromPos, moveFinal.ToPos, pieceType!.Value);
                }

                BoardDrawer.TryToGetMove(moveFinal.ToPos); //Для удаления предыдущего highlight'а
                HandleMove(moveFinal);
            }

            BoardDrawer.DrawKingCheck(GameManager.Board, GameManager.CurrentPlayer);
        } catch { }
    }

    #endregion

    #region PVP

    bool IsPromotion { get; set; } = false;
    
    async void MouseDownPvp(object obj)
    {
        if (obj is MouseButtonEventArgs e)
        {
            var position = UiChessManager.ToSquarePosition(e);
            Move? move = null;

            if (BoardDrawer.SelectedPosition is null)
            {
                IEnumerable<Move> moves = GameManager.LegalMovesForPieces(position);
                BoardDrawer.ShowPossibleMoves(position, moves);
            }
            else
            {
                move = BoardDrawer.TryToGetMove(position);
                if (move != null)
                {
                    if (move.Type == MoveType.PawnPromotion)
                    {
                        IsPromotion = true;
                        HandlePromotionMove(move.FromPos, move.ToPos);
                    }
                    else
                    {
                        HandleMove(move);
                    }
                }
            }

            BoardDrawer.DrawKingCheck(GameManager.Board, GameManager.CurrentPlayer);
            
            if (move != null)
            {
                
                if (move.Type != MoveType.PawnPromotion)
                {
                    await ChessClient.SendMessageAsync($"{ClientAction.MakeMove}{Client.MessageRegex}{move.FromPos}:{move.ToPos}:{StartPlayer}");
                }
                
                await GetMoveFromOpponent();
            }
            
            BoardDrawer.DrawKingCheck(GameManager.Board, GameManager.CurrentPlayer);

            IsPromotion = false;
        }
    }

    async Task GetMoveFromOpponent()
    {
        IsEnemyThinking = true;

        while (true)
        {
            var msg = await ChessClient.ReceiveMessageAsync();

            if (string.IsNullOrEmpty(msg))
            {
                continue;
            }

            if (msg.StartsWith(ClientAction.ExitRoom.ToString()))
            {
                MessageBox.Show(msg.Split(":", StringSplitOptions.RemoveEmptyEntries)[1]);
                Stop();
                MouseDownCommand = null;
                return;
            }

            if (msg.StartsWith(ClientAction.ShowRooms.ToString()))
            {
                MessageBox.Show("Wait till the end of game!");
            }

            if (msg.StartsWith(ClientAction.MakeMove.ToString()))
            {
                var enemyMove = msg!.Split(":", StringSplitOptions.RemoveEmptyEntries);

                var startRow = 7 - int.Parse(enemyMove[1][0].ToString());
                var startColumn = 7 - (enemyMove[1][1] - 'a');
                var endRow = 7 - int.Parse(enemyMove[2][0].ToString());
                var endColumn = 7 - (enemyMove[2][1] - 'a');

                var startPosition = new Position(startRow, startColumn);
                var endPosition = new Position(endRow, endColumn);

                IEnumerable<Move> moves = GameManager.LegalMovesForPieces(startPosition);

                var newMove = moves.First(m => m.ToPos == endPosition);

                if (newMove.Type == MoveType.PawnPromotion)
                {
                    newMove = new PawnPromotion(newMove.FromPos, newMove.ToPos,
                        Enum.Parse<PieceType>(enemyMove[3]));
                }

                HandleMove(newMove);

                break;
            }
        }

        IsEnemyThinking = false;
    }

    #endregion

    public void Dispose()
    {
        CancellationTokenSource?.Dispose();
        BotManager?.Dispose();
    }
}