using System.Numerics;
using AKG.Core.Objects;

namespace AKG.UI.MVVM.ViewModels.LightsVm;

public class LightViewModel : BaseViewModel
{

    public LightViewModel(Light light)
    {
        _position = light.Position;
        _color = light.Color;
        _intensity = light.Intensity;
    }

    public Light ToLight()
    {
        return new Light(_position, _color, _intensity);
    }

    private Vector3 _position;
    private Vector3 _color;
    private float _intensity;

    public Vector3 Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPropertyChanged();
        }
    }

    public Vector3 Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }

    public float Intensity
    {
        get => _intensity;
        set
        {
            _intensity = value;
            OnPropertyChanged();
        }
    }
}