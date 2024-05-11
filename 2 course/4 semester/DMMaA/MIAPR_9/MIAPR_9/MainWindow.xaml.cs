using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using MIAPR_9.NeuralNetwork;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MIAPR_9;

public partial class MainWindow
{
    const int ImageSize = 32;
    Bitmap _bitmap;
    NamedNeuralNetwork _network;

    int _counter;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    void ManualTeaching_Click(object sender, RoutedEventArgs e)
    {
        var setup = new ManualTeachingSetupWindow();
        if (setup.ShowDialog() != false)
        {
            _network = new(ImageSize * ImageSize, setup.Names.Select(x => x.Name).Distinct().ToList());
            LoadImageButton.IsEnabled = true;
        }
    }

    void TeachingButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _network.Teaching(BitmapConverter.ToInt32List(_bitmap), TeachingClassTextBox.Text);
        }
        catch
        {
            MessageBox.Show("Неверное имя класса", "MIAPR_9", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    void ClassificationButton_Click(object sender, RoutedEventArgs e) => ClassificationResultLabel.Text = _network.GetAnswer(BitmapConverter.ToInt32List(_bitmap));
    
    void AutoTeaching_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog
        {
            IsFolderPicker = true,
            Title = "Выберите папку для обучения",
        };

        _counter = 0;

        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            var directory = new DirectoryInfo(dialog.FileName!);
            
            List<string> neuronsDistinctNames = [];
            List<string> neuronsNames = [];
            foreach (var file in directory.GetFiles())
            {
                neuronsNames.Add(file.Name);
                neuronsDistinctNames.Add(file.Name.Split('-')[0]);
            }

            neuronsDistinctNames = neuronsDistinctNames.Distinct().ToList();

            _network = new (ImageSize * ImageSize, neuronsDistinctNames);
            while (!AutoEducationEnd(neuronsNames, directory))
            {
                foreach (var file in neuronsNames)
                {
                    _network.Teaching(GetElementFromPath(directory, file), file.Split('-')[0]);
                }
                _counter++;
            }

            MessageBox.Show($"Эпох: {_counter}");

            LoadImageButton.IsEnabled = true;
            SaveNetworkButton.IsEnabled = true;
        }
    }

    bool AutoEducationEnd(List<string> files, DirectoryInfo directory) =>
        files.All(file => file.Split('-')[0] == _network.GetAnswer(GetElementFromPath(directory, file)));

    List<int> GetElementFromPath(DirectoryInfo directory, string fileName) => 
        BitmapConverter.ToInt32List(BitmapConverter.Load($"{directory}/{fileName}", ImageSize));

    void LoadImageButton_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Images (*.bmp, *.png, *.jpg, *.jpeg)|*.bmp;*.png;*.jpg;*.jpeg|All files|*.*"
        };
        
        if (openFileDialog.ShowDialog() != false) 
        {
            _bitmap = BitmapConverter.Load(openFileDialog.FileName, ImageSize);
            CurrentImage.Source = BitmapConverter.ToBitmapImage(_bitmap);
            
            TeachingButton.IsEnabled = true;
            TeachingClassTextBox.IsEnabled = true;
            ClassificationButton.IsEnabled = true;
            SaveNetworkButton.IsEnabled = true;
        }
    }
    
    void SaveNetworkButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Filter = "Сохраненная ИНС(*.snn)|*.snn"
        };

        if (saveFileDialog.ShowDialog() == true)
        {

            using var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);

            var serializer = new XmlSerializer(typeof(NamedNeuralNetwork));
            serializer.Serialize(fileStream, _network);
        }
    }

    void OpenReadyNetwork_Click(object sender, RoutedEventArgs e)
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Сохраненная ИНС(*.snn)|*.snn"
        };
        
        if (openFileDialog.ShowDialog() == true)
        {
            using var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open);
            var serializer = new XmlSerializer(typeof(NamedNeuralNetwork));
            if (serializer.Deserialize(fileStream) is NamedNeuralNetwork namedNeuralNetwork)
            {
                _network = namedNeuralNetwork;
                LoadImageButton.IsEnabled = true;
                SaveNetworkButton.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Ошибка при десериализации");
            }
        }

    }

    private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        Close();
    }
}