using System.Collections.ObjectModel;
using System.Numerics;
using System.Windows.Input;
using AKG.Core.Objects;
using AKG.UI.LightsUI;
using AKG.UI.MVVM.Commands;

namespace AKG.UI.MVVM.ViewModels.LightsVm;

public class LightsListViewModel : BaseViewModel
{
    private LightViewModel? _selectedLight;

    public LightsListViewModel(List<Light> lights)
    {
        Lights = new();
        foreach (var light in lights)
        {
            Lights.Add(new(light));
        }
        EditLightCommand = new RelayCommand(_ => EditSelectedLight(), _ => SelectedLight != null);
        AddLightCommand = new RelayCommand(_ => AddNewLight());
        RemoveLightCommand = new RelayCommand(_ => RemoveSelectedLight(), _ => SelectedLight != null);
    }

    public ObservableCollection<LightViewModel> Lights { get; }

    public LightViewModel? SelectedLight
    {
        get => _selectedLight;
        set
        {
            _selectedLight = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public ICommand EditLightCommand { get; }
    public ICommand AddLightCommand { get; }
    public ICommand RemoveLightCommand { get; }

    private void AddNewLight()
    {
        var newLight = new Light(new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1.0f);

        var viewModelLight = new LightViewModel(newLight);
        
        var editWindow = new LightEditWindow
        {
            DataContext = new LightEditViewModel(viewModelLight)
        };

        if (editWindow.ShowDialog() == true)
        {
            Lights.Add(viewModelLight);
            SelectedLight = viewModelLight;
        }
    }

    private void RemoveSelectedLight()
    {
        if (SelectedLight != null)
        {
            Lights.Remove(SelectedLight);
            SelectedLight = null;
        }
    }

    private void EditSelectedLight()
    {
        if (SelectedLight == null) return;

        var editWindow = new LightEditWindow
        {
            DataContext = new LightEditViewModel(SelectedLight)
        };

        editWindow.ShowDialog();
    }
}