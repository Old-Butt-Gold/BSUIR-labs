using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ChessClient;

public class Client : IDisposable
{
    TcpClient _tcpClient = new(AddressFamily.InterNetwork);
    StreamReader _reader;
    StreamWriter _writer;
    bool _isReading = false;
    bool _isWriting = false;
    
    public const string MessageRegex = "#&#";
    
    public async Task ConnectAsync(IPAddress ipAddress, int port)
    {
        if (!IsConnected)
        {
            await _tcpClient.ConnectAsync(ipAddress, port);
            var stream = _tcpClient.GetStream();
            _reader = new StreamReader(stream, Encoding.UTF8);
            _writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
        }
        else
        {
            MessageBox.Show($"You are already connected to the server: {_tcpClient.Client.RemoteEndPoint}");
        }
    }

    public async Task DisconnectAsync()
    {
        if (IsConnected)
        {
            _reader?.Dispose();
            _writer?.Dispose();
            _tcpClient.Close();
            _tcpClient?.Dispose();
            _tcpClient = new(AddressFamily.InterNetwork);
        }
        else
        {
            MessageBox.Show("You are not connected to the server.");
        }
    }

    public bool IsConnected => _tcpClient.Connected;

    public async Task SendMessageAsync(string message)
    {
        if (!_isWriting)
        {
            _isWriting = true;
            await _writer.WriteLineAsync(message);
            _isWriting = false;
        }
    }

    public async Task<string?> ReceiveMessageAsync()
    {
        if (!_isReading)
        {
            _isReading = true;
            var message = await _reader.ReadLineAsync();
            _isReading = false;
            return message;
        }

        return null;
    }

    public void Dispose()
    {
        _reader?.Dispose();
        _writer?.Dispose();
        _tcpClient?.Close();
        _tcpClient?.Dispose();
    }
}