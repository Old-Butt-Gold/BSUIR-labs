using System.Windows;
using System.Windows.Input;
using AKG.Core.Objects;
using AKG.UI.MVVM.Commands;

namespace AKG.UI.MVVM.ViewModels.MaterialVm;

public class MaterialEditorViewModel : BaseViewModel
{
    private Material _material;
    public Material Material
    {
        get => _material;
        set { _material = value; OnPropertyChanged(); }
    }

    public ICommand SaveCommand { get; }

    public MaterialEditorViewModel(Material material)
    {
        Material = material;
        SaveCommand = new RelayCommand(_ => Save());
    }

    private void Save()
    {
        if (Application.Current.MainWindow!.DataContext is MainViewModel vm)
        {
            vm.UpdateView();
        }
    }
}