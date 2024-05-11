using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

class ServerObject
{
    readonly Socket _socketListener;
    readonly ConcurrentDictionary<string, ClientObject> _clients = new();

    public ServerObject(IPEndPoint ipEndPoint)
    {
        _socketListener = new(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _socketListener.Bind(ipEndPoint);
    }

    public void RemoveConnection(string id)
    {
        if (_clients.TryRemove(id, out var removedClient))
        {
            removedClient.Close();
        }
    }

    public void Listen()
    {
        try
        {
            _socketListener.Listen();

            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                Socket socketClient = _socketListener.Accept();

                ClientObject clientObject = new(socketClient, this);
                _clients.TryAdd(clientObject.Id, clientObject);

                Task.Run(clientObject.Process);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    public void BroadcastMessage(string message, string id)
    {
        foreach (var client in _clients.Values.Where(client => client.Id != id))
        {
            client.SendMessage(message);
        }
    }

    public void Disconnect()
    {
        foreach (var client in _clients.Values)
        {
            client.Close();
        }
        
        try
        {
            _socketListener.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            _socketListener.Close();
        }
    }
}