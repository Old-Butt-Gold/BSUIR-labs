using System.Windows;
using System.Windows.Input;
using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP;

public partial class ShapeEditorWindow
{
    private AbstractShape Shape { get; set; }

    public ShapeEditorWindow(AbstractShape shape)
    {
        InitializeComponent();
        Shape = shape;
        //DataContext меняет также, как и DependencyProperty (для DP необязательно INotifyPropertyChanged)
        DataContext = shape;
    }

    void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (!Shape.IsValid)
        {
            var list = Shape.GetErrors;
            MessageBox.Show("Присутствуют ошибки в данных: \n" + string.Join("\n", list), "Валидация", MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
        else
        {
            Close();
        }
    }

    void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) => ButtonBase_OnClick(sender, null);
}