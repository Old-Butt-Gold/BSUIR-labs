using System.Windows;
using System.Windows.Input;
using AKG.UI.MVVM.ViewModels.LightsVm;

namespace AKG.UI.LightsUI;

public partial class LightsSettingsWindow
{
    public LightsSettingsWindow()
    {
        InitializeComponent();
    }

    private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is LightsListViewModel { SelectedLight: not null } viewModel)
        {
            var editWindow = new LightEditWindow
            {
                DataContext = new LightEditViewModel(viewModel.SelectedLight)
            };

            editWindow.ShowDialog();
        }
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}