using System.Net;
using System.Net.Sockets;

class Program
{
    static Client? _client;
    public static void Main(string[] args)
    {
        try
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Введите IP адрес сервера:");
            string serverIp = Console.ReadLine()!;

            Console.WriteLine("Введите порт:");
            int port = int.Parse(Console.ReadLine()!);

            clientSocket.Connect(IPAddress.Parse(serverIp), port);

            _client = new Client(clientSocket);
            _client.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при запуске клиента: {ex.Message}");
        }
        finally
        {
            _client?.Close();
        }

        Console.WriteLine("Нажмите любую клавишу для выхода");
        Console.ReadKey();
    }
}