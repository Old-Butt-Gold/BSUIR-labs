using System.Windows;
using AKG.UI.MVVM.ViewModels;

namespace AKG.UI;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.Scene.CanvasHeight = (int)ImagePanel.ActualHeight;
            vm.Scene.CanvasWidth = (int)ImagePanel.ActualWidth;
        }
    }
}