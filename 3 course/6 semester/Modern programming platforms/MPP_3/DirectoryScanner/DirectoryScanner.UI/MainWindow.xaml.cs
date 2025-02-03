using DirectoryScanner.UI.ViewModel;

namespace DirectoryScanner.UI;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}