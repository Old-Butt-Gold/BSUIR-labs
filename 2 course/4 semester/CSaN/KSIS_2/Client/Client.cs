using System.Net.Sockets;
using System.Text;

class Client(Socket clientSocket)
{
    public void Start()
    {
        Console.WriteLine("Введите ваше имя:");
        string userName = Console.ReadLine()!;
        SendMessage(userName);
        Console.WriteLine($"Добро пожаловать, {userName}!");

        Task.Run(ReceiveMessage);

        while (true)
        {
            string message = Console.ReadLine()!;
            if (message.Equals("/exit", StringComparison.OrdinalIgnoreCase))
            {
                SendMessage("/exit");
                break;
            }

            SendMessage(message);
        }

    }

    void ReceiveMessage()
    {
        while (clientSocket.Connected)
        {
            StringBuilder sb = new();
            int bytesRead;
            byte[] inputData = new byte[1024];
            do
            {
                bytesRead = clientSocket.Receive(inputData);
                sb.Append(Encoding.UTF8.GetString(inputData, 0, bytesRead));
            } while (clientSocket.Available > 0 || bytesRead == 1024);

            Console.WriteLine(sb);
        }
    }

    void SendMessage(string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        clientSocket.Send(buffer);
    }

    public void Close()
    {
        try
        {
            clientSocket.Shutdown(SocketShutdown.Both);
        }
        finally
        {
            clientSocket.Close();
        }
    }
}