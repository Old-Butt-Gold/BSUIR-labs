using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

static class ComputerInfo
{
    public static string MAC { get; private set; }
    public static IPAddress IpAddress { get; private set; }
    public static string ComputerName { get; private set; }

    static ComputerInfo() => Update();

    public static void Update()
    {
        ComputerName = Dns.GetHostName();
        var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(x => x is
            { NetworkInterfaceType: NetworkInterfaceType.Ethernet});
        if (networkInterface is not null)
        {
            IdentifyIP(networkInterface);
            IdentifyMAC(networkInterface);
        }
    }

    static void IdentifyIP(NetworkInterface networkInterface)
    {
        foreach (var item in networkInterface.GetIPProperties().UnicastAddresses)
        {
            if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                IpAddress = item.Address;
        }
    }

    static void IdentifyMAC(NetworkInterface networkInterface)
    {
        byte[] bytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
        MAC = string.Join(":", bytes.Select(x => x.ToString("X2")));
    }

}