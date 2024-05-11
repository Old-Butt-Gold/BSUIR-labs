using System.Windows;

namespace KSIS_1;

public partial class ResultWindow : Window
{
    public ResultWindow(List<LocalNode> list)
    {
        InitializeComponent();
        ResultListView.ItemsSource = list;
    }
    
}