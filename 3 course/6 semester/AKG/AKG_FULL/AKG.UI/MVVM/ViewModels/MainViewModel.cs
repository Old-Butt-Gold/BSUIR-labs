using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AKG.Core.Enum;
using AKG.Core.ImageHelpers;
using AKG.Core.Objects;
using AKG.Core.Parser;
using AKG.Core.Renderer;
using AKG.UI.CameraUI;
using AKG.UI.LightsUI;
using AKG.UI.MaterialUI;
using AKG.UI.MVVM.Commands;
using AKG.UI.MVVM.ViewModels.CameraVm;
using AKG.UI.MVVM.ViewModels.LightsVm;
using AKG.UI.MVVM.ViewModels.MaterialVm;
using AKG.UI.Services.Implementations;
using AKG.UI.Services.Interfaces;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AKG.UI.MVVM.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Stopwatch _fpsStopwatch = Stopwatch.StartNew();

    private DateTime _lastClickTime = DateTime.MinValue;
    private const int DoubleClickThreshold = 300; // Порог времени для двойного клика (в миллисекундах)
    private Point _axisXEnd = new(70, 50);
    private Point _axisXNegativeEnd = new(70, 100);

    private Point _axisXNegativeText = new(112.5, 87.5);

    private Point _axisXText = new(112.5, 67.5);

    private Point _axisYEnd = new(50, 30);

    private Point _axisYNegativeEnd = new(50, 110);

    private Point _axisYNegativeText = new(67.5, 107.5);

    private Point _axisYText = new(67.5, 37.5);

    private Point _axisZEnd = new(45, 75);

    private Point _axisZNegativeEnd = new(45, 125);

    private Point _axisZNegativeText = new(37.5, 117.5);

    private Point _axisZText = new(37.5, 67.5);

    private Color _backgroundColor = Colors.LightGray;

    private Color _foregroundColor = Colors.Red;
    private double _fps;

    // Свойство для отображения FPS
    // Поля для FPS-счетчика
    private int _frameCount;

    private Color _highlightColor = Colors.Peru;

    private bool _isModelInfoVisible;

    // Поля для отслеживания состояния вращения
    private Point _lastMousePos;

    private string _selectedModelInfo = string.Empty;

    private RenderMode _selectedRenderMode;

    private WriteableBitmap? _writeableBitmap;

    public MainViewModel()
    {
        ColorPickerService = new ColorPickerService();
        Scene.Camera = new Camera();

        Scene.CanvasWidth = 800;
        Scene.CanvasHeight = 600;

        LoadFileCommand = new RelayCommand(_ => LoadFile());
        LoadBackgroundCommand = new RelayCommand(_ => LoadBackground());
        ClearSceneCommand = new RelayCommand(_ => ClearScene());
        EditCameraCommand = new RelayCommand(_ => EditCamera());
        EditLightsCommand = new RelayCommand(_ => EditLights());
        OpenMaterialEditorCommand = new RelayCommand(_ => OpenMaterialEditor());
        MouseWheelCommand = new RelayCommand(OnMouseWheel);
        MouseMoveCommand = new RelayCommand(OnMouseMove);
        MouseLeftButtonDownCommand = new RelayCommand(OnMouseLeftButtonDown);
        MouseRightButtonDownCommand = new RelayCommand(OnMouseRightButtonDown);
        KeyDownCommand = new RelayCommand(OnKeyDown);
        
        PickForegroundColorCommand = new RelayCommand(_ =>
        {
            var color = ColorPickerService.PickColor();
            if (color != null) ForegroundColor = color.Value;
        });

        PickBackgroundColorCommand = new RelayCommand(_ =>
        {
            var color = ColorPickerService.PickColor();
            if (color != null) BackgroundColor = color.Value;
        });

        PickHighlightColorCommand = new RelayCommand(_ =>
        {
            var color = ColorPickerService.PickColor();
            if (color != null) HighlightColor = color.Value;
        });

        ToggleModelInfoCommand = new RelayCommand(_ => IsModelInfoVisible = !IsModelInfoVisible);

        SelectedRenderMode = RenderMode.Texture;
        Scene.Lights.Add(new Light());
    }
    
    private void OpenMaterialEditor()
    {
        var window = new MaterialListWindow
        {
            DataContext = new MaterialListViewModel(Scene.Models)
        };
        window.Show();
    }

    public Scene Scene { get; set; } = new();

    public WriteableBitmap? WriteableBitmap
    {
        get => _writeableBitmap;
        set
        {
            _writeableBitmap = value;
            OnPropertyChanged(nameof(WriteableBitmap));
        }
    }

    public Color ForegroundColor
    {
        get => _foregroundColor;
        set
        {
            _foregroundColor = value;
            UpdateView();
            OnPropertyChanged(nameof(ForegroundColor));
        }
    }

    public Color BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            UpdateView();
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }

    public Color HighlightColor
    {
        get => _highlightColor;
        set
        {
            _highlightColor = value;
            UpdateView();
            OnPropertyChanged(nameof(HighlightColor));
        }
    }

    public string SelectedModelInfo
    {
        get => _selectedModelInfo;
        set
        {
            _selectedModelInfo = value;
            OnPropertyChanged(nameof(SelectedModelInfo));
        }
    }

    public double Fps
    {
        get => _fps;
        set
        {
            _fps = value;
            OnPropertyChanged(nameof(Fps));
        }
    }

    // Пример команд для загрузки, очистки и редактирования камеры
    public ICommand LoadFileCommand { get; }
    public ICommand LoadBackgroundCommand { get; }
    public ICommand ClearSceneCommand { get; }
    public ICommand EditCameraCommand { get; }

    public ICommand EditLightsCommand { get; }
    public ICommand OpenMaterialEditorCommand { get; }

    // Команды для событий мыши и клавиатуры
    public ICommand MouseWheelCommand { get; }
    public ICommand MouseMoveCommand { get; }

    public ICommand MouseLeftButtonDownCommand { get; }

    //public ICommand MouseLeftButtonUpCommand { get; }
    public ICommand MouseRightButtonDownCommand { get; }
    public ICommand KeyDownCommand { get; }

    // Комманды для цветовой палитры
    public ICommand PickForegroundColorCommand { get; }
    public ICommand PickBackgroundColorCommand { get; }
    public ICommand PickHighlightColorCommand { get; }
    public ICommand ToggleModelInfoCommand { get; }
    private float RotateSensitivity => MathF.PI / 360.0f;

    private IColorPickerService ColorPickerService { get; }

    public RenderMode SelectedRenderMode
    {
        get => _selectedRenderMode;
        set
        {
            _selectedRenderMode = value;
            UpdateView();
            OnPropertyChanged(nameof(SelectedRenderMode));
        }
    }


    public bool IsModelInfoVisible
    {
        get => _isModelInfoVisible;
        set
        {
            _isModelInfoVisible = value;
            OnPropertyChanged(nameof(IsModelInfoVisible));
        }
    }

    public Point AxisXNegativeEnd
    {
        get => _axisXNegativeEnd;
        set
        {
            _axisXNegativeEnd = value;
            OnPropertyChanged(nameof(AxisXNegativeEnd));
        }
    }

    public Point AxisYNegativeEnd
    {
        get => _axisYNegativeEnd;
        set
        {
            _axisYNegativeEnd = value;
            OnPropertyChanged(nameof(AxisYNegativeEnd));
        }
    }

    public Point AxisZNegativeEnd
    {
        get => _axisZNegativeEnd;
        set
        {
            _axisZNegativeEnd = value;
            OnPropertyChanged(nameof(AxisZNegativeEnd));
        }
    }

    public Point AxisXNegativeText
    {
        get => _axisXNegativeText;
        set
        {
            _axisXNegativeText = value;
            OnPropertyChanged(nameof(AxisXNegativeText));
        }
    }

    public Point AxisYNegativeText
    {
        get => _axisYNegativeText;
        set
        {
            _axisYNegativeText = value;
            OnPropertyChanged(nameof(AxisYNegativeText));
        }
    }

    public Point AxisZNegativeText
    {
        get => _axisZNegativeText;
        set
        {
            _axisZNegativeText = value;
            OnPropertyChanged(nameof(AxisZNegativeText));
        }
    }

    public Point AxisXEnd
    {
        get => _axisXEnd;
        set
        {
            _axisXEnd = value;
            OnPropertyChanged(nameof(AxisXEnd));
        }
    }

    public Point AxisYEnd
    {
        get => _axisYEnd;
        set
        {
            _axisYEnd = value;
            OnPropertyChanged(nameof(AxisYEnd));
        }
    }

    public Point AxisZEnd
    {
        get => _axisZEnd;
        set
        {
            _axisZEnd = value;
            OnPropertyChanged(nameof(AxisZEnd));
        }
    }

    public Point AxisXText
    {
        get => _axisXText;
        set
        {
            _axisXText = value;
            OnPropertyChanged(nameof(AxisXText));
        }
    }

    public Point AxisYText
    {
        get => _axisYText;
        set
        {
            _axisYText = value;
            OnPropertyChanged(nameof(AxisYText));
        }
    }

    public Point AxisZText
    {
        get => _axisZText;
        set
        {
            _axisZText = value;
            OnPropertyChanged(nameof(AxisZText));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void LoadBackground()
    {
        using var dlg = new CommonOpenFileDialog();
        dlg.Filters.Add(new CommonFileDialogFilter("HDR Files", "*.hdr"));
        if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            try
            {
                Scene.Background = new HdRiBackground();
                Scene.Background.LoadFromHdrFile(dlg.FileName!);
                UpdateView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки фона: " + ex.Message);
            }
    }

    private void LoadFile()
    {
        using var dlg = new CommonOpenFileDialog();
        dlg.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.obj"));
        if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            try
            {
                var loadedModel = ObjParser.Parse(dlg.FileName!);
                if (loadedModel.Materials is null)
                    MessageBox.Show("Данные о mtl файле не найдены", "Внимание", MessageBoxButton.OK);
                WriteableBitmap ??= new WriteableBitmap(
                    Scene.CanvasWidth, Scene.CanvasHeight, 96, 96, PixelFormats.Bgra32, null);

                // Добавляем модель в сцену и делаем её выбранной
                Scene.Models.Add(loadedModel);
                Scene.SelectedModel = loadedModel;
                UpdateView();
                OnPropertyChanged(nameof(Scene));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки файла: " + ex.Message);
            }
    }

    private void ClearScene()
    {
        Scene.Models.Clear();
        Scene.Background = null;
        Scene.SelectedModel = null;
        Scene.Camera = new Camera();

        TextureLoader.ClearCache();
        TextureSampler.ClearCache();

        UpdateView();
        OnPropertyChanged(nameof(Scene));
        GC.Collect();
    }

    private void EditCamera()
    {
        // Создаём окно для редактирования параметров камеры
        var cameraWindow = new CameraSettingsWindow
        {
            DataContext = new CameraSettingsViewModel(Scene.Camera)
        };
        // Показываем окно как модальное
        if (cameraWindow.ShowDialog() == true)
        {
            UpdateView();
            OnPropertyChanged(nameof(Scene));
        }
    }

    private void EditLights()
    {
        var lightsWindow = new LightsSettingsWindow
        {
            DataContext = new LightsListViewModel(Scene.Lights)
        };

        if (lightsWindow.ShowDialog() == true)
        {
            Scene.Lights.Clear();

            var updatedLights = (lightsWindow.DataContext as LightsListViewModel)?.Lights;

            foreach (var light in updatedLights!)
            {
                Scene.Lights.Add(light.ToLight());
            }
            
            UpdateView();
            OnPropertyChanged(nameof(Scene));
        }
    }
    
    private void OnMouseWheel(object? parameter)
    {
        if (parameter is MouseWheelEventArgs e)
        {
            if (Scene.SelectedModel != null &&
                (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                if (e.Delta > 0)
                    Scene.SelectedModel.Scale += Scene.SelectedModel.Delta;
                else
                    Scene.SelectedModel.Scale -= Scene.SelectedModel.Delta;
            }
            else
            {
                Scene.Camera.Radius -= e.Delta / 1000.0f;
                if (Scene.Camera.Radius < Scene.Camera.ZNear)
                    Scene.Camera.Radius = Scene.Camera.ZNear;
                if (Scene.Camera.Radius > Scene.Camera.ZFar)
                    Scene.Camera.Radius = Scene.Camera.ZFar;
            }

            e.Handled = true;

            UpdateView();
            OnPropertyChanged(nameof(Scene));
        }
    }

    private void OnMouseMove(object? parameter)
    {
        if (parameter is MouseEventArgs e)
        {
            if (Scene.SelectedModel != null)
                // Вращение модели
                if (e.LeftButton == MouseButtonState.Pressed && e.RightButton != MouseButtonState.Pressed)
                {
                    var currentPos = e.GetPosition(null);
                    var delta = currentPos - _lastMousePos;
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                        Scene.SelectedModel.Rotation = new Vector3(
                            Scene.SelectedModel.Rotation.X,
                            Scene.SelectedModel.Rotation.Y,
                            Scene.SelectedModel.Rotation.Z - (float)delta.X * RotateSensitivity);
                    else
                        Scene.SelectedModel.Rotation = new Vector3(
                            Scene.SelectedModel.Rotation.X + (float)delta.Y * RotateSensitivity,
                            Scene.SelectedModel.Rotation.Y + (float)delta.X * RotateSensitivity,
                            Scene.SelectedModel.Rotation.Z);

                    _lastMousePos = currentPos;

                    UpdateView();
                    OnPropertyChanged(nameof(Scene));
                }

            // Вращение камеры
            if (e.RightButton == MouseButtonState.Pressed && e.LeftButton != MouseButtonState.Pressed)
            {
                var currentPos = e.GetPosition(null);

                var xOffset = (float)(currentPos.X - _lastMousePos.X);
                var yOffset = (float)(currentPos.Y - _lastMousePos.Y);


                Scene.Camera.Zeta -= yOffset * 0.005f;
                Scene.Camera.Phi += xOffset * 0.005f;

                if (Scene.Camera.Zeta >= Math.PI)
                    Scene.Camera.Zeta = (float)Math.PI - 0.01f;
                if (Scene.Camera.Zeta <= 0)
                    Scene.Camera.Zeta = 0.01f;

                _lastMousePos = currentPos;
                UpdateView();
                OnPropertyChanged(nameof(Scene));
            }
        }
    }

    private void OnMouseLeftButtonDown(object? parameter)
    {
        if (parameter is MouseButtonEventArgs e)
        {
            var currentTime = DateTime.Now;
            var timeSinceLastClick = (currentTime - _lastClickTime).TotalMilliseconds;

            if (timeSinceLastClick <= DoubleClickThreshold)
            {
                // Это двойной клик
                HandleDoubleClick(e);
                _lastClickTime = DateTime.MinValue; // Сбрасываем время последнего клика
            }
            else
            {
                // Это одиночный клик
                _lastClickTime = currentTime;
                HandleSingleClick(e);
            }
        }
    }

    private void HandleSingleClick(MouseButtonEventArgs e)
    {
        // Логика для одиночного клика
        _lastMousePos = e.GetPosition(null);
        if (e.OriginalSource is UIElement uiElement)
        {
            uiElement.Focus();
        }
    }

    private void HandleDoubleClick(MouseButtonEventArgs e)
    {
        var clickPoint = e.GetPosition(null);

        // Получаем элемент Image

        if (Application.Current.MainWindow!.FindName("ImgDisplay") is Image image)
        {
            // Определяем, какой источник света был выбран
            var pickedLight = Scene.PickLight(clickPoint, image);

            if (pickedLight != null)
            {
                // Открываем окно редактирования для выбранного источника света
                var editWindow = new LightEditWindow
                {
                    DataContext = new LightEditViewModel(new(pickedLight))
                };

                if (editWindow.ShowDialog() == true)
                {
                    var newLight = (editWindow.DataContext as LightEditViewModel)?.SelectedLight;
                    pickedLight.Color = newLight!.Color;
                    pickedLight.Position = newLight.Position;
                    pickedLight.Intensity = newLight.Intensity;
                    UpdateView();
                    OnPropertyChanged(nameof(Scene));
                }
            }
        }
    }
    
    private void OnMouseRightButtonDown(object? parameter)
    {
        if (parameter is MouseButtonEventArgs e)
        {
            _lastMousePos = e.GetPosition(null);
            var clickPoint = _lastMousePos;
            var pickedModel = Scene.PickModel(clickPoint);
            Scene.SelectedModel = pickedModel;
            UpdateView();
            OnPropertyChanged(nameof(Scene));
        }
    }

    private void OnKeyDown(object? parameter)
    {
        if (parameter is KeyEventArgs e)
        {
            if (Scene.SelectedModel != null)
            {
                if (e.Key == Key.Delete)
                {
                    Scene.Models.Remove(Scene.SelectedModel);
                    Scene.SelectedModel = Scene.Models.FirstOrDefault();

                    UpdateView();
                    OnPropertyChanged(nameof(Scene));
                    return;
                }

                var step = Scene.SelectedModel.GetOptimalTranslationStep();

                switch (e.Key)
                {
                    case Key.Right:
                        Scene.SelectedModel.Translation += new Vector3(step.X, 0, 0);
                        break;
                    case Key.Left:
                        Scene.SelectedModel.Translation += new Vector3(-step.X, 0, 0);
                        break;
                    case Key.Up:
                        Scene.SelectedModel.Translation += new Vector3(0, step.Y, 0);
                        break;
                    case Key.Down:
                        Scene.SelectedModel.Translation += new Vector3(0, -step.Y, 0);
                        break;
                    case Key.S:
                        Scene.SelectedModel.Translation += new Vector3(0, 0, -step.Z);
                        break;
                    case Key.W:
                        Scene.SelectedModel.Translation += new Vector3(0, 0, step.Z);
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Left:
                        Scene.Camera.Target += new Vector3(-0.5f, 0, 0);
                        break;
                    case Key.Right:
                        Scene.Camera.Target += new Vector3(0.5f, 0, 0);
                        break;
                    case Key.Up:
                        Scene.Camera.Target += new Vector3(0.0f, 0.5f, 0);
                        break;
                    case Key.Down:
                        Scene.Camera.Target += new Vector3(0.0f, -0.5f, 0);
                        break;
                }
            }

            UpdateView();
            OnPropertyChanged(nameof(Scene));
        }
    }

    public void UpdateView()
    {
        RendererFacade.Render(Scene, WriteableBitmap, BackgroundColor, ForegroundColor, HighlightColor,
            SelectedRenderMode);

        UpdateSelectedModelInfo();

        OnPropertyChanged(nameof(WriteableBitmap));

        _frameCount++;
        var elapsed = _fpsStopwatch.Elapsed.TotalSeconds;
        if (elapsed >= 1.0)
        {
            Fps = _frameCount / elapsed;
            _frameCount = 0;
            _fpsStopwatch.Restart();
        }

        UpdateAxisWidget();
    }

    private void UpdateAxisWidget()
    {
        var camera = Scene.Camera;
        var viewMatrix = camera.GetViewMatrix();

        var axisX = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitX, viewMatrix));
        var axisY = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitY, viewMatrix));
        var axisZ = Vector3.Normalize(Vector3.TransformNormal(Vector3.UnitZ, viewMatrix));

        const double scale = 60;
        const double textOffset = 5;

        // Положительные оси
        AxisXEnd = new Point(75 + axisX.X * scale, 75 - axisX.Y * scale);
        AxisYEnd = new Point(75 + axisY.X * scale, 75 - axisY.Y * scale);
        AxisZEnd = new Point(75 + axisZ.X * scale, 75 - axisZ.Y * scale);

        // Отрицательные оси
        AxisXNegativeEnd = new Point(75 - axisX.X * scale, 75 + axisX.Y * scale);
        AxisYNegativeEnd = new Point(75 - axisY.X * scale, 75 + axisY.Y * scale);
        AxisZNegativeEnd = new Point(75 - axisZ.X * scale, 75 + axisZ.Y * scale);

        // Положительный текст
        AxisXText = new Point(AxisXEnd.X + axisX.X * textOffset, AxisXEnd.Y - axisX.Y * textOffset);
        AxisYText = new Point(AxisYEnd.X + axisY.X * textOffset, AxisYEnd.Y - axisY.Y * textOffset);
        AxisZText = new Point(AxisZEnd.X + axisZ.X * textOffset, AxisZEnd.Y - axisZ.Y * textOffset);

        // Текст для отрицательных осей
        AxisXNegativeText = new Point(AxisXNegativeEnd.X + axisX.X * textOffset,
            AxisXNegativeEnd.Y - axisX.Y * textOffset);
        AxisYNegativeText = new Point(AxisYNegativeEnd.X + axisY.X * textOffset,
            AxisYNegativeEnd.Y - axisY.Y * textOffset);
        AxisZNegativeText = new Point(AxisZNegativeEnd.X + axisZ.X * textOffset,
            AxisZNegativeEnd.Y - axisZ.Y * textOffset);
    }

    /// <summary>
    ///     Обновляет строку с информацией о выбранной модели.
    /// </summary>
    private void UpdateSelectedModelInfo()
    {
        if (Scene.SelectedModel == null)
        {
            SelectedModelInfo = "No model selected.";
            return;
        }

        var model = Scene.SelectedModel;
        var rotXDeg = NormalizeAngle(model.Rotation.X * (180.0 / MathF.PI));
        var rotYDeg = NormalizeAngle(model.Rotation.Y * (180.0 / MathF.PI));
        var rotZDeg = NormalizeAngle(model.Rotation.Z * (180.0 / MathF.PI));

        SelectedModelInfo =
            $"Vertices: {model.OriginalVertices.Count}\n" +
            $"Faces: {model.Faces.Count}\n" +
            $"Scale: {model.Scale:F10}\n" +
            $"Delta: {model.Delta:F10}\n" +
            $"Translation: ({model.Translation.X:F2}, {model.Translation.Y:F2}, {model.Translation.Z:F2})\n" +
            $"Rotation: (X:{rotXDeg:F0}°, Y:{rotYDeg:F0}°, Z:{rotZDeg:F0}°)\n" +
            $"Model Size: (X: {model.Max.X - model.Min.X:F2}, Y: {model.Max.Y - model.Min.Y:F2}, Z: {model.Max.Z - model.Min.Z:F2});";

        double NormalizeAngle(double angle)
        {
            angle %= 360;
            if (angle > 180)
                angle -= 360;
            else if (angle <= -180)
                angle += 360;
            return angle;
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}