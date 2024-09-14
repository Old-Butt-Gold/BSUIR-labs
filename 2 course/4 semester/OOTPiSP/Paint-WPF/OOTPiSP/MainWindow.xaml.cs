using SharedComponents;
using SharedComponents.AbstractClasses;

namespace OOTPiSP;

public partial class MainWindow
{
    public MainViewModel MainViewModel { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        AbstractShape.Canvas = Canvas;
        MainViewModel.LoadCurrentFiguresDynamic(this);
    }
}