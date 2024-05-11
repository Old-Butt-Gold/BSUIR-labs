using System.Windows;

namespace MIAPR_9;

public partial class ManualTeachingSetupWindow
{
    public ManualTeachingSetupWindow()
    {
        InitializeComponent();
        SetNamesGridSize();
    }

    public List<NameDataGridRow> Names { get; private set; } = new();

    void SetNamesGridSize()
    {
        Names = [];
        for (var i = 0; i < ElementsCountUpDown.Value; i++)
        {
            Names.Add(new((i + 1).ToString()));
        }
        
        NamesDataGrid.ItemsSource = Names;
        NamesDataGrid.ColumnWidth = Width;
    }

    void OKButton_Click(object sender, RoutedEventArgs e) => DialogResult = true;

    void ElementsCountUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (NamesDataGrid != null)
        {
            SetNamesGridSize();
        }
    }
    
    public class NameDataGridRow(string name)
    {
        public string Name { get; set; } = name;
    }
}