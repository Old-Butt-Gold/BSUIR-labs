using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KSIS_1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    readonly HttpClient _client = new();
    bool _isStartedInterface;
    bool _isStartedConnections;

    async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (_isStartedInterface)
        {
            MessageBox.Show("Дождитесь сбора информации", "Внимание");
            return;
        }

        NetworkInterfacesListView.Items.Clear();
        LocalNetworkInterfacesListView.Items.Clear();
        _isStartedInterface = true;
        await Task.Run(() =>
        {
            DisplayComputerInformation();
            DisplayNetworkInterfacesInfo();
            DisplayLocalNetworkInterfacesInfo();
            MessageBox.Show("Сбор информации завершен", "Внимание");
        });
        _isStartedInterface = false;
    }

    void DisplayComputerInformation()
    {
        ComputerInfo.Update();
        Dispatcher.Invoke(() =>
        {
            ComputerNameTextBlock.Text = ComputerInfo.ComputerName;
            MacAddressTextBlock.Text = ComputerInfo.MAC;
            IpAddressTextBlock.Text = ComputerInfo.IpAddress.ToString();
        });
    }

    void DisplayNetworkInterfacesInfo()
    {
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            foreach (var item in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var currentInterface = GetInfoAboutIp(item.Address.ToString());
                    if (currentInterface != null)
                    {
                        Dispatcher.Invoke(() => { NetworkInterfacesListView.Items.Add(currentInterface); });
                    }
                }
            }
        }
    }

    void DisplayLocalNetworkInterfacesInfo()
    {
        List<string> localAddresses = GetLocalAddresses();
        foreach (var ipAddress in localAddresses)
        {
            var currentInterface = GetInfoAboutIp(ipAddress);
            if (currentInterface != null)
            {
                Dispatcher.Invoke(() => { LocalNetworkInterfacesListView.Items.Add(currentInterface); });
            }
        }
    }

    List<string> GetLocalAddresses()
    {
        List<string> result = new List<string>();
        foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            var items = networkInterface.GetIPProperties().UnicastAddresses
                .Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork && IsInLocalRange(x.Address));
            result.AddRange(items.Select(ip => ip.Address.ToString()));
        }

        return result;

        bool IsInLocalRange(IPAddress address)
        {
            byte[] bytes = address.GetAddressBytes();
            return bytes[0] == 192 && bytes[1] == 168; // Checking for 192.168.x.x range
        }
    }

    Info? GetInfoAboutIp(string ip)
    {
        foreach (var item in NetworkInterface.GetAllNetworkInterfaces())
        {
            var ipInfos = item.GetIPProperties().UnicastAddresses;
            foreach (var ipInfo in ipInfos)
            {
                if (ipInfo.Address.ToString() == ip)
                {
                    var type = item.GetIPProperties().IsDynamicDnsEnabled ? "Динамический" : "Статический";
                    return new Info(item.Name,
                        string.Join(":", item.GetPhysicalAddress().GetAddressBytes().Select(x => x.ToString("X2"))),
                        item.Description, ipInfo.Address, ipInfo.IPv4Mask, item.OperationalStatus, type);
                }
            }
        }

        return null;
    }

    string? GetMacAddress(string ipAddress)
    {
        try
        {
            byte[] macAddr = new byte[6];
            uint macAddrLen = (uint)macAddr.Length;
            int dest = BitConverter.ToInt32(IPAddress.Parse(ipAddress).GetAddressBytes(), 0);

            return NativeMethods.SendARP(dest, 0, macAddr, ref macAddrLen) == 0
                ? string.Join(":", macAddr.Select(x => x.ToString("X2")))
                : null;
        }
        catch
        {
            return null;
        }
    }

    int CreateBaseIp(IPAddress ip, int mask)
    {
        byte[] ipBytes = ip.GetAddressBytes();
        byte[] maskBytes = CreateMask(mask);

        byte[] networkAddressBytes = new byte[ipBytes.Length];
        for (int i = 0; i < ipBytes.Length; i++)
        {
            networkAddressBytes[ipBytes.Length - i - 1] = (byte)(ipBytes[i] & maskBytes[i]);
        }

        return (int)BitConverter.ToUInt32(networkAddressBytes, 0);

        static byte[] CreateMask(int mask)
        {
            byte[] bytes = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                if (mask >= 8)
                {
                    bytes[i] = 255;
                    mask -= 8;
                }
                else
                {
                    int value = 0;
                    for (int j = 0; j < mask; j++)
                    {
                        value = (value << 1) | 1;
                    }

                    bytes[i] = (byte)(value << (8 - mask));
                    break;
                }
            }

            return bytes;
        }
    }

    string GetDeviceName(string ipAddress)
    {
        try
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);
            return hostEntry.HostName;
        }
        catch (Exception)
        {
            return "Неизвестно";
        }
    }

    int CountConsecutiveOnes(string mask)
    {
        string[] octets = mask.Split('.');
        if (octets.Length != 4) throw new ArgumentException("Неправильный формат IPv4-адреса");

        int consecutiveOnes = 0;
        foreach (string octet in octets)
        {
            if (!int.TryParse(octet, out int value) || value < 0 || value > 255)
            {
                throw new ArgumentException("Неправильный формат IPv4-адреса");
            }

            string binary = Convert.ToString(value, 2);
            foreach (char bit in binary)
            {
                if (bit == '1')
                {
                    consecutiveOnes++;
                }
                else
                {
                    break;
                }
            }
        }

        return consecutiveOnes;
    }

    async void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (e is { ChangedButton: MouseButton.Left, LeftButton: MouseButtonState.Pressed })
        {
            if (sender is ListView { SelectedItem: Info item })
            {
                if (!_isStartedConnections)
                {
                    MessageBox.Show("Начинается сбор информации об узлах в подсети!", "Внимание");
                    _isStartedConnections = true;
                    var mask = CountConsecutiveOnes(item.IPv4Mask.ToString());
                    int minIpNum = CreateBaseIp(item.IpAddress, mask);
                    int maxIpNum = minIpNum + (int)Math.Pow(2, 32 - mask) - 1;
                    List<LocalNode> nodes = new List<LocalNode>();

                    await Task.Run(() =>
                    {
                        Parallel.For(minIpNum, maxIpNum, i =>
                        {
                            byte[] ipAddressBytes = BitConverter.GetBytes(i);
                            Array.Reverse(ipAddressBytes);
                            string newIpAddress = (new IPAddress(ipAddressBytes)).ToString();

                            string? macAddress = GetMacAddress(newIpAddress);
                            if (macAddress != null)
                            {
                                string deviceName = GetDeviceName(newIpAddress);
                                if (deviceName == "Неизвестно")
                                {
                                    var task = GetNameByDictionary(macAddress);
                                    deviceName = task.Result;
                                }

                                nodes.Add(new(newIpAddress, deviceName, macAddress));
                            }
                        });
                    });
                    ResultWindow resultWindow = new ResultWindow(nodes);
                    resultWindow.Show();
                    _isStartedConnections = false;
                }
                else
                {
                    MessageBox.Show("Дождитесь предыдущего сбора информации об узла в подсети!", "Внимание");
                }
            }
        }
    }

    async Task<string> GetNameByDictionary(string macAddress)
    {
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            var response = await _client.GetAsync($"https://api.macvendors.com/{macAddress}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
        return "Неизвестно";
    }
}