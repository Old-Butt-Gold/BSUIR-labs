namespace ChessServer;

public class ChessRoom {
    public string Name { get; }
    List<ChessPlayer> Players { get; } = [];

    public int Count => Players.Count;

    public ChessRoom(string name) 
    {
        Name = name;
    }

    public ChessPlayer? WhitePlayer { get; set; }
    public ChessPlayer? BlackPlayer { get; set; }

    public void AddPlayer(ChessPlayer chessPlayer) => Players.Add(chessPlayer);

    public void RemovePlayer(ChessPlayer chessPlayer)
    {
        Players.Remove(chessPlayer);
        if (chessPlayer.Equals(WhitePlayer))
        {
            WhitePlayer = null;
        }

        if (chessPlayer.Equals(BlackPlayer))
        {
            BlackPlayer = null;
        }
    }
}