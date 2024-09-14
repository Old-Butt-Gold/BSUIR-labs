using System.Net.Sockets;
using System.Text;

namespace ChessServer;

public class ChessPlayer : IDisposable, IEquatable<ChessPlayer>
{
    public RoomNameString RoomNameString { get; set; }
    string Id { get; } = Guid.NewGuid().ToString();

    public TcpClient TcpClient { get; set; }

    public StreamReader StreamReader { get; set; }
    public StreamWriter StreamWriter { get; set; }

    public ChessPlayer(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
        StreamReader = new StreamReader(tcpClient.GetStream(), Encoding.UTF8);
        StreamWriter = new StreamWriter(tcpClient.GetStream(), Encoding.UTF8)
        {
            AutoFlush = true,
        };
    }

    public void Dispose()
    {
        TcpClient?.Dispose();
        StreamReader?.Dispose();
        StreamWriter?.Dispose();
    }

    public bool Equals(ChessPlayer? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ChessPlayer)obj);
    }

    public override int GetHashCode() => Id.GetHashCode();
}