using System.Numerics;
using System.Windows;
using AKG.UI.MVVM.ViewModels.LightsVm;
using AKG.UI.Services.Implementations;
using AKG.UI.Services.Interfaces;

namespace AKG.UI.LightsUI;

public partial class LightEditWindow
{
    public LightEditWindow()
    {
        InitializeComponent();
    }

    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is LightEditViewModel vm)
        {
            vm.ApplyChanges();
        }
        DialogResult = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
    
    private readonly IColorPickerService _colorPicker = new ColorPickerService();
    
    private void PickColor_Click(object sender, RoutedEventArgs e)
    {
        // Вызываем диалог выбора цвета
        var chosenColor = _colorPicker.PickColor();
        if (chosenColor.HasValue)
        {
            // Преобразуем выбранный System.Windows.Media.Color (0-255) в нормализованный вектор (0-1)
            var normalized = new Vector3(chosenColor.Value.ScR, chosenColor.Value.ScG, chosenColor.Value.ScB);

            // Обновляем TempColor в DataContext (LightEditViewModel)
            if (DataContext is LightEditViewModel vm)
            {
                vm.TempColor = normalized;
            }
        }
    }
}