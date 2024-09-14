using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ChessClient;
using ChessLogic.Bot;
using ChessLogic.CoordinateClasses;
using Microsoft.Xaml.Behaviors;

namespace ChessUI;

public partial class MainWindow
{
    Client ChessClient { get; set; } = new();
    ChessViewModel ViewModel { get; }
    
    public MainWindow()
    {
        InitializeComponent();
        ViewModel = new ChessViewModel
        {
            UiChessManager =
            {
                HighLightGrid = this.HighLightGrid,
                PieceGrid = this.PieceGrid,
                MenuContainer = this.MenuContainer,
                BoardGrid = this.BoardGrid
            },
        };
        ViewModel.ChessClient = ChessClient;
        InputBindings.Add(new KeyBinding(ViewModel.ShowHelpCommand, Key.F1, ModifierKeys.None));
        DataContext = ViewModel;
    }
    
    async void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        if (ChessClient.IsConnected)
        {
            await ChessClient.SendMessageAsync(ClientAction.ExitRoom.ToString());
        }
        
        ViewModel?.Dispose();
        ChessClient?.Dispose();
    }

    async void BotButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (ChessClient.IsConnected)
        {
            await ChessClient.SendMessageAsync(ClientAction.ExitRoom.ToString());
        }
        
        BotDifficulty botDifficulty = (BotDifficulty)BotDifficultyComboBox.SelectedIndex;
        Player player = (Player)(PlayerComboBox.SelectedIndex + 1);
        
        ViewModel.StartBot(player, botDifficulty);
    }

    void BoardGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        //Из-за того, что Binding присвваивается сразу к null-команде
        if (ViewModel.MouseDownCommand != null && ViewModel.MouseDownCommand.CanExecute(sender))
        {
            ViewModel.MouseDownCommand?.Execute(e);
        }
    }

    async void Connect_OnClick(object sender, RoutedEventArgs e)
    {
        if (!IPAddress.TryParse(ServerIpTextBox.Text, out var ipAddress))
        {
            MessageBox.Show("Invalid IP address.");
            return;
        }

        if (!int.TryParse(PortTextBox.Text, out var port))
        {
            MessageBox.Show("Invalid port number.");
            return;
        }

        var button = e.Source as Button;
        button.IsEnabled = false;
        try
        {
            await ChessClient.ConnectAsync(ipAddress, port);
            await GetFreeRooms();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Connection failed: {ex.Message}");
        }
        finally
        {
            button.IsEnabled = true;
        }
    }

    async void Disconnect_OnClick(object sender, RoutedEventArgs e)
    {
        if (ChessClient.IsConnected)
        {
            await ChessClient.SendMessageAsync($"{ClientAction.ExitRoom}");
        }
        
        await ChessClient.DisconnectAsync();
    }

    async void Refresh_OnClick(object sender, RoutedEventArgs e)
    {
        if (!ChessClient.IsConnected)
        {
            MessageBox.Show("You are not connected to the server.");
            return;
        }
        
        var button = e.Source as Button;
        button!.IsEnabled = false;
        await GetFreeRooms();
        button.IsEnabled = true;
    }

    async Task GetFreeRooms()
    {
        await ChessClient.SendMessageAsync(ClientAction.ShowRooms.ToString());
        var message = await ChessClient.ReceiveMessageAsync();
        
        if (string.IsNullOrEmpty(message))
            return;

        var rooms = message?.Split(Client.MessageRegex, StringSplitOptions.RemoveEmptyEntries);

        List<RoomInfo> roomInfos = [];

        foreach (var room in rooms!.Skip(1))
        {
            var components = room.Split(":", StringSplitOptions.RemoveEmptyEntries);
            roomInfos.Add(new RoomInfo(components[0], int.Parse(components[1])));
        }

        AvailableRoomsListBox.ItemsSource = roomInfos;
    }
    
    class RoomInfo
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public RoomInfo(string name, int count) => (Name, Count) = (name, count);
    }

    async void Room_Action(object sender, RoutedEventArgs e)
    {
        if (!ChessClient.IsConnected)
        {
            MessageBox.Show("You are not connected to the server.");
            return;
        }

        var item = (ClientAction) RoomActionComboBox.SelectedIndex;
        var button = e.Source as Button;
        button.IsEnabled = false;
        
        if (item == ClientAction.CreateRoom)
        {
            await ChessClient.SendMessageAsync($"{ClientAction.CreateRoom}{Client.MessageRegex}{RoomNameTextBox.Text}");
            var message = await ChessClient.ReceiveMessageAsync();

            var messageComponents = message!.Split(":", StringSplitOptions.RemoveEmptyEntries);
            await GetFreeRooms();
            MessageBox.Show(messageComponents[1]);
            
            if (bool.Parse(messageComponents[2]))
            {
                ActionButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
                while (true)
                {
                    var msg = await ChessClient.ReceiveMessageAsync();
                    if (msg is null)
                    {
                        continue;
                    }
                    
                    if (msg.StartsWith(ClientAction.ConnectRoom.ToString()))
                    {
                        ViewModel.StartPvp(Player.White);
                        
                        await GetFreeRooms();
                        
                        MessageBox.Show("The game has started");
                        break;
                    }

                    if (msg.StartsWith(ClientAction.ExitRoom.ToString()))
                    {
                        MessageBox.Show(msg.Split(":",StringSplitOptions.RemoveEmptyEntries)[1]);
                        break;
                    }

                    if (msg.StartsWith(ClientAction.ShowRooms.ToString()))
                    {
                        await GetFreeRooms();
                    }
                }
            }
        }

        if (item == ClientAction.ConnectRoom)
        {
            await ChessClient.SendMessageAsync($"{ClientAction.ConnectRoom}{Client.MessageRegex}{RoomNameTextBox.Text}");
            var message = await ChessClient.ReceiveMessageAsync();
           
            var messageComponents = message!.Split(":", StringSplitOptions.RemoveEmptyEntries);
            await GetFreeRooms();

            if (bool.Parse(messageComponents[2]))
            {
                ActionButton.Visibility = Visibility.Collapsed;
                ExitButton.Visibility = Visibility.Visible;
                ViewModel.StartPvp(Player.Black);
            }
            
            MessageBox.Show(messageComponents[1]);
        }

        button.IsEnabled = true;
    }

    async void Exit_Action(object sender, RoutedEventArgs e)
    {
        await ChessClient.SendMessageAsync(ClientAction.ExitRoom.ToString());
        
        ViewModel.Stop();
        ActionButton.Visibility = Visibility.Visible;
        ExitButton.Visibility = Visibility.Collapsed;
    }
}
