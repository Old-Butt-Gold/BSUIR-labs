using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ChessClient;
using ChessLogic.CoordinateClasses;
using ChessLogic.Pieces;

namespace ChessServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        Server server = new();
        await server.Start(5555);
    }
}

class Server
{
    ConcurrentDictionary<string, ChessRoom> _gameRooms = new();

    StreamWriter Log { get; } = new(new FileStream("log.txt", FileMode.Append)) { AutoFlush = true};

    const string MessageRegex = "#&#";
    
    public async Task Start(int port)
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
        await Log.WriteLineAsync($"{DateTime.UtcNow} – Server is started...");
        tcpListener.Start();

        while (true)
        {
            var tcpClient = await tcpListener.AcceptTcpClientAsync();
            await Log.WriteLineAsync($"{DateTime.UtcNow} – Connected User: {tcpClient.Client.RemoteEndPoint}");
            ProcessPlayer(new ChessPlayer(tcpClient));
        }
    }

    async void ProcessPlayer(ChessPlayer chessPlayer)
    {
        chessPlayer.RoomNameString = new RoomNameString(null);
        try
        {
            while (chessPlayer.TcpClient.Connected)
            {
                var message = await chessPlayer.StreamReader.ReadLineAsync();
                
                if (string.IsNullOrEmpty(message))
                {
                    await chessPlayer.StreamWriter.WriteLineAsync(string.Empty); //перестало нормально отсоединяться, теперь работает
                    continue;
                }

                if (message.StartsWith(ClientAction.ShowRooms.ToString()))
                {
                    StringBuilder roomNames = new(ClientAction.ShowRooms + MessageRegex);
                    foreach (var item in _gameRooms.Values)
                    {
                        roomNames.Append(item.Name + ":" + item.Count + MessageRegex);
                    }
                    await chessPlayer.StreamWriter.WriteLineAsync(roomNames);
                }

                if (message.StartsWith(ClientAction.ExitRoom.ToString()))
                {
                    if (chessPlayer.RoomNameString.Name != null)
                    {
                        if (_gameRooms.TryRemove(chessPlayer.RoomNameString.Name, out var room))
                        {
                            if (room.WhitePlayer != null && room.WhitePlayer.Equals(chessPlayer))
                            {
                                await room.WhitePlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:You exit the game");

                                if (room.BlackPlayer != null)
                                {
                                    await room.BlackPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:Your enemy has exit the game");
                                    room.BlackPlayer.RoomNameString.Name = null;
                                }
            
                                room.WhitePlayer.RoomNameString.Name = null;
                            }

                            if (room.BlackPlayer != null && room.BlackPlayer.Equals(chessPlayer))
                            {
                                await room.BlackPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:You exit the game");
                                
                                if (room.WhitePlayer != null)
                                {
                                    await room.WhitePlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:Your enemy has exit the game");
                                    room.WhitePlayer.RoomNameString.Name = null;
                                }

                                room.BlackPlayer.RoomNameString.Name = null;
                            }

                        }
                    }
                }
                
                if (message.StartsWith(ClientAction.CreateRoom.ToString()))
                {
                    var command = message.Split(MessageRegex, StringSplitOptions.RemoveEmptyEntries);
                    // Format: create_room#&#room_name
                    var room = new ChessRoom(command[1]);
                    if (_gameRooms.TryAdd(command[1], room))
                    {
                        room.WhitePlayer = chessPlayer;
                        room.AddPlayer(chessPlayer);
                        room.WhitePlayer.RoomNameString.Name = command[1];
                        await chessPlayer.StreamWriter.WriteLineAsync($"{ClientAction.CreateRoom}:Room was created. Waiting for Players...:true");
                    }
                    else
                    {
                        await chessPlayer.StreamWriter.WriteLineAsync($"{ClientAction.CreateRoom}:Room with this name already exists.:false");
                    }
                }

                if (message.StartsWith(ClientAction.ConnectRoom.ToString()))
                {
                    var command = message.Split(MessageRegex, StringSplitOptions.RemoveEmptyEntries);
                    //connect_room#&#room_name
                    if (_gameRooms.TryGetValue(command[1], out var gameRoom))
                    {
                        if (gameRoom.Count < 2)
                        {
                            gameRoom.BlackPlayer = chessPlayer;
                            gameRoom.AddPlayer(chessPlayer);
                            gameRoom.BlackPlayer.RoomNameString.Name = gameRoom.Name;
                            await gameRoom.BlackPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ConnectRoom}:Game has started.:true");
                            await gameRoom.WhitePlayer!.StreamWriter.WriteLineAsync($"{ClientAction.ConnectRoom}:Game has started.:true");
                        }
                        else
                        {
                            await chessPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ConnectRoom}:Room with this name is full.:false");
                        }
                    }
                    else
                    {
                        await chessPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ConnectRoom}:Room with this name does not exist.:false");
                    }
                }

                if (message.StartsWith(ClientAction.MakeMove.ToString()))
                {
                    if (chessPlayer.RoomNameString.Name != null)
                    {
                        _gameRooms.TryGetValue(chessPlayer.RoomNameString.Name, out var room);
                        
                        var str = message.Split(MessageRegex, StringSplitOptions.RemoveEmptyEntries);
                        var move = str[1];
                        var moveComponents = move.Split(":", StringSplitOptions.RemoveEmptyEntries);
                        Player currentPlayer = Enum.Parse<Player>(moveComponents[2]);
                        
                        var startPosition = moveComponents[0];
                        var endPosition = moveComponents[1];

                        var promotionType = string.Empty;
                        if (moveComponents.Length == 4)
                        {
                            promotionType += ":" + Enum.Parse<PieceType>(moveComponents[3]);
                        }

                        if (currentPlayer == Player.White)
                        {
                            await room!.BlackPlayer!.StreamWriter.WriteLineAsync($"{ClientAction.MakeMove}:{startPosition}:{endPosition}{promotionType}");
                        }
                        else
                        {
                            await room!.WhitePlayer!.StreamWriter.WriteLineAsync($"{ClientAction.MakeMove}:{startPosition}:{endPosition}{promotionType}");
                        }
                    } 
                }
            }
        }
        catch (Exception ex)
        {
            await Log.WriteLineAsync($"{DateTime.UtcNow} – An error occurred while processing player {chessPlayer.TcpClient.Client.RemoteEndPoint}: {ex.Message}");
        }
        finally
        {
            if (chessPlayer.RoomNameString.Name != null)
            {
                if (_gameRooms.TryRemove(chessPlayer.RoomNameString.Name, out var room))
                {
                    if (room.WhitePlayer != null && room.WhitePlayer.Equals(chessPlayer))
                    {
                        if (room.WhitePlayer.TcpClient.Connected)
                        {
                            await room.WhitePlayer.StreamWriter.WriteLineAsync(
                                $"{ClientAction.ExitRoom}:You exit the game");
                        }

                        if (room.BlackPlayer != null)
                        {
                            await room.BlackPlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:Your enemy has exit the game");
                            room.BlackPlayer.RoomNameString.Name = null;
                        }
            
                        room.WhitePlayer.RoomNameString.Name = null;
                    }

                    if (room.BlackPlayer != null && room.BlackPlayer.Equals(chessPlayer))
                    {
                        if (room.BlackPlayer.TcpClient.Connected)
                        {
                            await room.BlackPlayer.StreamWriter.WriteLineAsync(
                                $"{ClientAction.ExitRoom}:You exit the game");
                        }

                        if (room.WhitePlayer != null)
                        {
                            await room.WhitePlayer.StreamWriter.WriteLineAsync($"{ClientAction.ExitRoom}:Your enemy has exit the game");
                            room.WhitePlayer.RoomNameString.Name = null;
                        }

                        room.BlackPlayer.RoomNameString.Name = null;
                    }

                }
            }
            chessPlayer.Dispose();
        }
    }

}