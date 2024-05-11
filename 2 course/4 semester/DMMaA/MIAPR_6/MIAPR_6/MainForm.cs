using System;
using System.Windows.Forms;

namespace MIAPR_6;

public partial class MainForm : Form
{
    double[,] Distances { get; set; }
    const int RandomSeed = 15;

    bool _isStart;

    public MainForm() => InitializeComponent();
    
    double[,] RandomGrid(int size)
    {
        dataGridView.ColumnCount = size;
        dataGridView.RowCount = size;
        
        var result = new double[size, size];

        if (_isStart)
        {
            var rnd = new Random();
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    result[j, i] = result[i, j] = Math.Round(rnd.NextDouble() * RandomSeed, 2);
                }
            }
        }
        else
        {
            result[0, 1] = result[1, 0] = 5;
            result[0, 2] = result[2, 0] = 0.5;
            result[0, 3] = result[3, 0] = 2;
            result[1, 2] = result[2, 1] = 1;
            result[1, 3] = result[3, 1] = 0.6;
            result[2, 3] = result[3, 2] = 2.5;
            _isStart = true;
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                dataGridView[i, j].Value = result[i, j];
            }
        }
        
        if (radioBtnMaximum.Checked)
        {
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    result[j, i] = result[i, j] = 1 / result[i, j];
                }
            }
        }
        return result;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        int size = (int)numericUpDownGridSize.Value;
        Distances = RandomGrid(size);
        
        var hierarchical = new HierarchicalGrouping(Distances, size);
        hierarchical.FindGroups();
        
        ChartDrawer drawer = new ChartDrawer();
        drawer.Draw(chart1, hierarchical.GetGroups());
    }

    void dataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e) =>
        dataGridView[e.RowIndex, e.ColumnIndex].Value = dataGridView[e.ColumnIndex, e.RowIndex].Value;
}