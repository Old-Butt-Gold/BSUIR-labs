using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AKG.Core.Parser;
using AKG.Core.Renderer;
using Microsoft.WindowsAPICodePack.Dialogs;
using Vector = System.Windows.Vector;

namespace AKG.UI;

public partial class MainWindow
{
    private ObjModel? ObjModel { get; set; }
    private WriteableBitmap? Wb { get; set; }

    private float RotateSensitivity { get; init; } = MathF.PI / 360.0f;
    
    private bool _isRotating;
    private Point _lastMousePos;
    
    private Color ForegroundSelectedColor { get; set; } = Colors.Red;
    private Color BackgroundSelectedColor { get; set; } = Colors.White;
    
    public MainWindow()
    {
        InitializeComponent();
        WindowState = WindowState.Maximized;
    }
    
    private void ObjModel_TransformationChanged(object? sender, EventArgs e)
    {
        RedrawModel();
        UpdateModelInfo();
    }

    private void LoadFile_OnClick(object sender, RoutedEventArgs e)
    {
        using var dlg = new CommonOpenFileDialog();
        dlg.Filters.Add(new CommonFileDialogFilter("OBJ Files", "*.obj"));
        if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
        {
            try
            {
                ObjModel = ObjParser.Parse(dlg.FileName!);

                int width = (int)(ImagePanel.ActualWidth > 0 ? ImagePanel.ActualWidth : 800);
                int height = (int)(ImagePanel.ActualHeight > 0 ? ImagePanel.ActualHeight : 600);
                ObjModel.WindowSize = new(width, height);

                Wb = new WriteableBitmap(ObjModel.WindowSize.Width, ObjModel.WindowSize.Height, 96, 96, PixelFormats.Bgra32, null);
                ImgDisplay.Source = Wb;
                
                ObjModel.TransformationChanged += ObjModel_TransformationChanged;

                ObjModel.Scale = ObjModel.Delta * 10.0f; // вызовет UpdateImage -> RedrawModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки файла: " + ex.Message);
            }
        }
    }
    
    private void RedrawModel()
    {
        if (Wb == null || ObjModel == null) return;
        WireframeRenderer.ClearBitmap(Wb, BackgroundSelectedColor);
        WireframeRenderer.DrawWireframe(ObjModel, Wb, ForegroundSelectedColor);
    }
    
    private void FileClear_OnClick(object sender, RoutedEventArgs e)
    {
        if (Wb != null)
        {
            WireframeRenderer.ClearBitmap(Wb, BackgroundSelectedColor);
            ObjModel = null;
        }
    }

    private void ImagePanel_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (ObjModel != null)
        {
            if (e.Delta > 0)
            {
                ObjModel.Scale += ObjModel.Delta;
            }
            else
            {
                ObjModel.Scale -= ObjModel.Delta;
            }
        }
    }
    
    private void ImagePanel_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        ImagePanel.Focus();
        _isRotating = true;
        _lastMousePos = e.GetPosition(ImagePanel);
        ImagePanel.CaptureMouse();
    }

    private void ImagePanel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        _isRotating = false;
        ImagePanel.ReleaseMouseCapture();
    }
    
    private void ImagePanel_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_isRotating && ObjModel != null)
        {
            Point currentPos = e.GetPosition(ImagePanel);
            Vector delta = currentPos - _lastMousePos;

            // Если нажата клавиша Shift, вращаем по оси Z, иначе по X и Y.
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                ObjModel.Rotation = new Vector3(
                    ObjModel.Rotation.X,
                    ObjModel.Rotation.Y,
                    ObjModel.Rotation.Z - (float)delta.X * RotateSensitivity);
            }
            else
            {
                ObjModel.Rotation = new Vector3(
                    ObjModel.Rotation.X + (float)delta.Y * RotateSensitivity,
                    ObjModel.Rotation.Y + (float)delta.X * RotateSensitivity,
                    ObjModel.Rotation.Z);
            }
            _lastMousePos = currentPos;
        }
    }

    private void ImagePanel_KeyDown(object sender, KeyEventArgs e)
    {
        if (ObjModel is null) return;

        var optimalStep = ObjModel.GetOptimalTranslationStep();
        
        switch (e.Key)
        {
            case Key.Right:
                ObjModel.Translation += new Vector3(optimalStep.X, 0, 0);
                break;
            case Key.Left:
                ObjModel.Translation += new Vector3(-optimalStep.X, 0, 0);
                break;
            case Key.Up:
                ObjModel.Translation += new Vector3(0, optimalStep.Y, 0);
                break;
            case Key.Down:
                ObjModel.Translation += new Vector3(0, -optimalStep.Y, 0);
                break;
            case Key.S:
                ObjModel.Translation += new Vector3(0, 0, optimalStep.Z);
                break;
            case Key.W:
                ObjModel.Translation += new Vector3(0, 0, -optimalStep.Z);
                break;
        }
    }

    private void ForegroundColor_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new System.Windows.Forms.ColorDialog();
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            ForegroundSelectedColor = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            RedrawModel(); 
        }
    }

    private void BackgroundColor_OnClick(object sender, RoutedEventArgs e)
    {
        var dialog = new System.Windows.Forms.ColorDialog();
        if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            BackgroundSelectedColor = Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            RedrawModel(); 
        }
    }
    
    private void UpdateModelInfo()
    {
        if (ObjModel == null) return;

        double rotXDeg = NormalizeAngle(ObjModel.Rotation.X * (180.0 / Math.PI));
        double rotYDeg = NormalizeAngle(ObjModel.Rotation.Y * (180.0 / Math.PI));
        double rotZDeg = NormalizeAngle(ObjModel.Rotation.Z * (180.0 / Math.PI));

        string info = $"Vertices: {ObjModel.OriginalVertices.Count}\n" +
                      $"Faces: {ObjModel.Faces.Count}\n" +
                      $"Scale: {ObjModel.Scale:F10}\n" +
                      $"Delta: {ObjModel.Delta:F10}\n" +
                      $"Translation: ({ObjModel.Translation.X:F2}, {ObjModel.Translation.Y:F2}, {ObjModel.Translation.Z:F2})\n" +
                      $"Rotation: (X:{rotXDeg:F0}°, Y:{rotYDeg:F0}°, Z:{rotZDeg:F0}°)\n" +
                      $"Model Size: (X: {ObjModel.Max.X - ObjModel.Min.X:F2}, Y: {ObjModel.Max.Y - ObjModel.Min.Y:F2}, Z: {ObjModel.Max.Z - ObjModel.Min.Z:F2});";
        ModelInfoText.Text = info;
        
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

    private void ToggleModelInfoPopup(object sender, RoutedEventArgs e)
    {
        ModelInfoPopup.IsOpen = !ModelInfoPopup.IsOpen;
    }
}