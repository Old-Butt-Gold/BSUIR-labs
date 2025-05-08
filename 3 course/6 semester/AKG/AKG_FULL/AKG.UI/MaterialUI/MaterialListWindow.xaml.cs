using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AKG.Core.Objects;
using AKG.UI.MVVM.ViewModels.MaterialVm;

namespace AKG.UI.MaterialUI;

public partial class MaterialListWindow
{
    public MaterialListWindow()
    {
        InitializeComponent();
    }

    private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (DataContext is MaterialListViewModel vm)
        {
            if (sender is ListView listView)
            {
                var selectedItem = listView.SelectedItem as KeyValuePair<string, Material>?;
                var value = selectedItem!.Value;
                
                var window = new MaterialEditorWindow
                {
                    DataContext = new MaterialEditorViewModel(value.Value)
                };
                window.Show();
            }
        }
    }
}