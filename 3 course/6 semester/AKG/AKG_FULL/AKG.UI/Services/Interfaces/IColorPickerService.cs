using System.Windows.Media;

namespace AKG.UI.Services.Interfaces;

public interface IColorPickerService
{
    /// <summary>
    ///     Показывает диалог выбора цвета с заданным начальным цветом.
    ///     Если пользователь выбирает цвет, возвращает его; иначе — null.
    /// </summary>
    Color? PickColor();
}