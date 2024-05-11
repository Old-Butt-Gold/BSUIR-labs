using System.Net.Sockets;
using System.Text;

class ClientObject(Socket socketClient, ServerObject serverObject)
{
    public string Id { get; } = Guid.NewGuid().ToString();

    public void Process()
    {
        string userName = "";
        try
        {
            userName = ReceiveMessage();
            string message = $"{userName} вошел в чат; ID: {Id}";

            serverObject.BroadcastMessage(message, Id);
            Console.WriteLine(message);

            while (true)
            {
                var receivedMessage = ReceiveMessage();
                if (receivedMessage == "/exit")
                {
                    throw new SocketException();
                }
                
                message = $"{userName}: {receivedMessage}";
                Console.WriteLine(message);
                serverObject.BroadcastMessage(message, Id);
            }
        }
        catch (SocketException)
        {
            var message = $"{userName} покинул чат";
            Console.WriteLine(message);
            serverObject.BroadcastMessage(message, Id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            serverObject.RemoveConnection(Id);
        }
    }

    public string ReceiveMessage()
    {
        StringBuilder stringBuilder = new();
        int bytesRead;
        byte[] buffer = new byte[1024];
        do
        {
            bytesRead = socketClient.Receive(buffer, SocketFlags.None);
            stringBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
        } while (socketClient.Available > 0 || bytesRead == 1024);

        return stringBuilder.ToString();
    }

    public void SendMessage(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        socketClient.Send(buffer, SocketFlags.None);
    }

    public void Close()
    {
        try
        {
            socketClient.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            socketClient.Close();
        }
    }
}