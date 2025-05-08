using System.Numerics;
using AKG.Core.Objects;

namespace AKG.UI.MVVM.ViewModels.CameraVm;

public class CameraSettingsViewModel : BaseViewModel
{
    private readonly Camera _camera;
    private float _aspect;

    // Для отмены действий
    private Vector3 _eye;
    private float _fov;
    private float _phi;
    private float _radius;
    private Vector3 _target;
    private Vector3 _up;
    private float _zeta;
    private float _zFar;
    private float _zNear;

    public CameraSettingsViewModel(Camera camera)
    {
        _camera = camera;
        _eye = camera.Eye;
        _target = camera.Target;
        _up = camera.Up;
        _fov = camera.Fov;
        _aspect = camera.Aspect;
        _zNear = camera.ZNear;
        _zFar = camera.ZFar;
        _radius = camera.Radius;
        _zeta = camera.Zeta;
        _phi = camera.Phi;
    }

    public Vector3 Eye
    {
        get => _eye;
        set
        {
            _eye = value;
            OnPropertyChanged();
        }
    }

    public Vector3 Target
    {
        get => _target;
        set
        {
            _target = value;
            OnPropertyChanged();
        }
    }

    public Vector3 Up
    {
        get => _up;
        set
        {
            _up = value;
            OnPropertyChanged();
        }
    }

    public float Fov
    {
        get => _fov;
        set
        {
            _fov = value;
            OnPropertyChanged();
        }
    }

    public float Aspect
    {
        get => _aspect;
        set
        {
            _aspect = value;
            OnPropertyChanged();
        }
    }

    public float ZNear
    {
        get => _zNear;
        set
        {
            _zNear = value;
            OnPropertyChanged();
        }
    }

    public float ZFar
    {
        get => _zFar;
        set
        {
            _zFar = value;
            OnPropertyChanged();
        }
    }

    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
            OnPropertyChanged();
        }
    }

    public float Zeta
    {
        get => _zeta;
        set
        {
            _zeta = value;
            OnPropertyChanged();
        }
    }

    public float Phi
    {
        get => _phi;
        set
        {
            _phi = value;
            OnPropertyChanged();
        }
    }

    public void CommitChanges()
    {
        _camera.Eye = Eye;
        _camera.Target = Target;
        _camera.Up = Up;
        _camera.Fov = Fov;
        _camera.Aspect = Aspect;
        _camera.ZNear = ZNear;
        _camera.ZFar = ZFar;
        _camera.Radius = Radius;
        _camera.Zeta = Zeta;
        _camera.Phi = Phi;
    }
}