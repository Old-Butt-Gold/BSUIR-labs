using Renci.SshNet;

class Program
{
    static void Main()
    {
        Console.Write("Enter your SSH info in the format username@host: ");
        string sshInfo = Console.ReadLine()!;

        if (TryParseSshInfo(sshInfo, out var username, out var host))
        {
            Console.Write($"{sshInfo}'s password: ");
            var password = ReadPasswordFromConsole();
            Console.WriteLine();

            using var client = CreateSshClient(host, username, password);
            if (client != null)
            {
                ExecuteSshCommands(client, host, username);
            }
            else
            {
                Console.WriteLine($"Unable to connect to {host}.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input format. Please provide username@host.");
        }
        Console.ReadKey();
    }

    static bool TryParseSshInfo(string sshInfo, out string username, out string host)
    {
        username = string.Empty;
        host = string.Empty;

        string[] sshParts = sshInfo.Split('@');

        if (sshParts.Length == 2)
        {
            username = sshParts[0];
            host = sshParts[1];
            return true;
        }

        return false;
    }

    static string ReadPasswordFromConsole()
    {
        string password = string.Empty;
        ConsoleKeyInfo keyInfo;
        do
        {
            keyInfo = Console.ReadKey(true);
            if (keyInfo.Key != ConsoleKey.Enter)
            {
                password += keyInfo.KeyChar;
            }
        } while (keyInfo.Key != ConsoleKey.Enter);

        return password;
    }
    
    static SshClient? CreateSshClient(string host, string username, string password)
    {
        try
        {
            var client = new SshClient(host, username, password);
            client.Connect();
            return client.IsConnected ? client : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while connecting: {ex.Message}");
            return null;
        }
    }

    static void ExecuteSshCommands(SshClient client, string host, string username)
    {
        Console.WriteLine($"Connected to {host} as {username}.");
        PrintConnectionInfo(client);

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{username}@{host}");
            Console.ResetColor();
            Console.Write(":");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("~");
            Console.ResetColor();
            Console.Write("$ ");
            string command = Console.ReadLine()!;
            if (command == "exit")
                break;

            ExecuteCommand(client, command, username);
        }

        Console.WriteLine($"Connected to {host} closed.");
        
        void PrintConnectionInfo(SshClient client)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            using var result = client.RunCommand("lsb_release -a");
            Console.WriteLine(result.Result);

            using var memoryInfoResult = client.RunCommand("free -h");
            Console.WriteLine("Memory Information:");
            Console.WriteLine(memoryInfoResult.Result);

            using var cpuInfoResult = client.RunCommand("lshw -class processor");
            Console.WriteLine("CPU Information:");
            Console.WriteLine(cpuInfoResult.Result);

            using var cpuUsageResult = client.RunCommand("top -bn1 | grep 'Cpu(s)' | sed 's/.*, *\\([0-9.]*\\)%* id.*/\\1/' | awk '{print 100 - $1\"%\"}'");
            Console.WriteLine("CPU Usage:");
            Console.WriteLine(cpuUsageResult.Result);

            using var diskInfoResult = client.RunCommand("df -h");
            Console.WriteLine("Disk Information:");
            Console.WriteLine(diskInfoResult.Result);

            Console.WriteLine("Last login:");
            using var lastLogInfo = client.RunCommand("last -1");
            Console.WriteLine(lastLogInfo.Result);

            Console.ResetColor();
        }
    }

    static void ExecuteCommand(SshClient client, string command, string username)
    {
        using var result = client.RunCommand(command);
        if (result.Result != string.Empty)
        {
            Console.WriteLine(result.Result);
        }
        else if (result.Error != string.Empty)
        {
            HandleError(client, result, command, username);
        }
    }

    static void HandleError(SshClient client, SshCommand result, string command, string username)
    {
        if (result.Error.Contains("sudo"))
        {
            Console.Write($"[sudo] Password for {username}: ");
            string sudoPassword = ReadPasswordFromConsole();
            Console.WriteLine();
            using var cmd = client.RunCommand($"echo -e '{sudoPassword}\n' | sudo -S {command}");
            if (cmd.ExitStatus == 0)
            {
                if (cmd.Result != string.Empty)
                {
                    Console.WriteLine(cmd.Result);
                }
            }
            else
            {
                Console.WriteLine(cmd.Error);
            }
        }
        else
        {
            Console.WriteLine(result.Error);
        }
    }
}