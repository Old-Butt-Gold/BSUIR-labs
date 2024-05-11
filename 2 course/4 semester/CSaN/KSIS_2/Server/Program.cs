using System.Net;
using System.Net.NetworkInformation;

class Program
{
    static void Main()
    {
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        var tcpListeners = ipGlobalProperties.GetActiveTcpListeners().Select(x => x.Port);
        
        Console.WriteLine("Занятые порты: ");
        Console.WriteLine(string.Join("\n", tcpListeners));
        
        Console.WriteLine("Введите порт для прослушивания сервера: ");
        int port;
        while (!int.TryParse(Console.ReadLine(), out port) || !IsPortAvailable(port))
        {
            Console.WriteLine("Порт неверен или занят, попробуйте еще раз");
        }

        IPEndPoint ipEndPoint = new(IPAddress.Any, port);

        ServerObject server = new ServerObject(ipEndPoint);
        server.Listen();
    }

    static bool IsPortAvailable(int port)
    {
        IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
        IPEndPoint[] tcpListeners = ipGlobalProperties.GetActiveTcpListeners();

        return tcpListeners.All(endPoint => endPoint.Port != port);
    }
}