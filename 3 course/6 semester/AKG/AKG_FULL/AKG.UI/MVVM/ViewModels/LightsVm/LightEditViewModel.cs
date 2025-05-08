using System.Numerics;

namespace AKG.UI.MVVM.ViewModels.LightsVm;

public class LightEditViewModel : BaseViewModel
{
    private LightViewModel _selectedLight;
    private Vector3 _tempColor;
    private Vector3 _tempPosition;
    private float _tempIntensity;

    public LightEditViewModel(LightViewModel selectedLight)
    {
        _selectedLight = selectedLight;
        _tempPosition = selectedLight.Position;
        _tempColor = selectedLight.Color;
        _tempIntensity = selectedLight.Intensity;
    }

    public LightViewModel SelectedLight => _selectedLight;

    public Vector3 TempPosition
    {
        get => _tempPosition;
        set
        {
            _tempPosition = value;
            OnPropertyChanged();
        }
    }

    public Vector3 TempColor
    {
        get => _tempColor;
        set
        {
            _tempColor = value;
            OnPropertyChanged();
        }
    }

    public float TempIntensity
    {
        get => _tempIntensity;
        set
        {
            _tempIntensity = value;
            OnPropertyChanged();
        }
    }

    public void ApplyChanges()
    {
        _selectedLight.Position = _tempPosition;
        _selectedLight.Color = _tempColor;
        _selectedLight.Intensity = _tempIntensity;
    }
}