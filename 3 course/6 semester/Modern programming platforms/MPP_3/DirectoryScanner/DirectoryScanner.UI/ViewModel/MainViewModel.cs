using System.Windows.Input;
using DirectoryScanner.Core.Models;
using DirectoryScanner.UI.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DirectoryScanner.UI.ViewModel;

public class MainViewModel : BaseViewModel
{
    private DirectoryNodeViewModel? _rootNode;
    private bool _isScanning;
    private CancellationTokenSource _cts;

    public DirectoryNodeViewModel? RootNode
    {
        get => _rootNode;
        set
        {
            _rootNode = value;
            OnPropertyChanged();
        }
    }

    public bool IsScanning
    {
        get => _isScanning;
        set
        {
            _isScanning = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectDirectoryCommand { get; }
    public ICommand CancelCommand { get; }

    public MainViewModel()
    {
        SelectDirectoryCommand = new RelayCommand(async () => await SelectDirectory());
        CancelCommand = new RelayCommand(Cancel);
    }

    private async Task SelectDirectory()
    {
        using var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
        dialog.InitialDirectory = Environment.CurrentDirectory;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            IsScanning = true;
            _cts = new CancellationTokenSource();

            DirectoryNode? root = null;

            try
            {
                root = await Task.Run(() => new Core.DirectoryScanner().Scan(dialog.FileName!, _cts.Token));
            }
            catch (OperationCanceledException) { }

            RootNode = root != null ? new(root, null!) : null;
            IsScanning = false;
        }
    }

    private void Cancel()
    {
        _cts?.Cancel();
    }
}