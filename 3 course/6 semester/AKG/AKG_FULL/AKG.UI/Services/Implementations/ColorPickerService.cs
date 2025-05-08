using System.Windows.Forms;
using System.Windows.Media;
using AKG.UI.Services.Interfaces;

namespace AKG.UI.Services.Implementations;

public class ColorPickerService : IColorPickerService
{
    private static ColorDialog ColorDialog { get; } = new();

    public Color? PickColor()
    {
        if (ColorDialog.ShowDialog() == DialogResult.OK)
            return Color.FromArgb(ColorDialog.Color.A, ColorDialog.Color.R, ColorDialog.Color.G, ColorDialog.Color.B);

        return null;
    }
}