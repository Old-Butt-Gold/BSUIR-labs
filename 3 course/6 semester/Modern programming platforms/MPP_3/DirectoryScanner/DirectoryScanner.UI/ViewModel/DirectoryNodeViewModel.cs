using System.Collections.ObjectModel;
using DirectoryScanner.Core.Models;

namespace DirectoryScanner.UI.ViewModel;

public class DirectoryNodeViewModel : BaseViewModel
{
    private readonly DirectoryNode _model;
    private readonly DirectoryNodeViewModel? _parent;

    public DirectoryNode Model => _model;

    public string Name => _model.Name;
    public string SizeText => $"{_model.TotalSize:N0} bytes";
    public string PercentageText => _parent != null ? $"{Percentage:F2}%" : "";
    public double Percentage => CalculatePercentage();

    public ObservableCollection<BaseViewModel> Children { get; } = [];

    public DirectoryNodeViewModel(DirectoryNode model, DirectoryNodeViewModel parent)
    {
        _model = model;
        _parent = parent;

        foreach (var file in model.Files)
            Children.Add(new FileNodeViewModel(file, this));

        foreach (var subDir in model.Subdirectories)
            Children.Add(new DirectoryNodeViewModel(subDir, this));
    }

    private double CalculatePercentage()
    {
        if (_parent == null || _parent._model.TotalSize == 0)
            return 0;
        return (double)_model.TotalSize / _parent._model.TotalSize * 100;
    }
}