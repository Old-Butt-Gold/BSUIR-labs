using SharedComponents.AbstractClasses;
using SharedComponents.Instruments;
using SharedComponents.Interfaces;

namespace AdapterCryptorForm;

public class AdapterCryptorPlugin : IPluginFunctionality
{
    CryptField Adaptee { get; set; } = new();

    public string Name => "Шифрование XML";
        
    public void SaveToFile(List<AbstractShape> abstractShapes)
    {
        MyXMLSerializer myXmlSerializer = new();
        var fileName = myXmlSerializer.Serialize(abstractShapes);

        if (fileName != string.Empty)
        {
            DialogResult result;
            using (var sourceStream = new FileStream(fileName, FileMode.Open))
            {
                result = Adaptee.ShowDialog();

                if (result == DialogResult.Yes)
                {
                    var encryptedStream = Adaptee.PostprocessorSave(sourceStream);

                    using FileStream fileEncrypted = File.Create(fileName + ".crp");

                    encryptedStream.CopyTo(fileEncrypted);

                    MessageBox.Show($"Шифрование прошло успешно");
                }
            }

            if (result == DialogResult.Yes)
            {
                File.Delete(fileName);
            }
        }
    }

    public (bool result, List<AbstractShape>? abstractShapes) LoadFile(Dictionary<object, AbstractFactory> dictionary)
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "CRP файлы (*.crp)|*.crp|XML файлы (*.xml)|*.xml"
        };
        List<AbstractShape> abstractShapes = [];
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            using FileStream sourceStream = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate);

            var result = Adaptee.ShowDialog();
            MyXMLSerializer myXmlSerializer = new();

            var listShapes = myXmlSerializer.Deserialize(
                result == DialogResult.Yes 
                ? Adaptee.PostprocessorLoad(sourceStream) 
                : sourceStream);

            if (listShapes is not null)
            {
                abstractShapes.Clear();
                AbstractShape.Canvas.Children.Clear();
                foreach (var item in listShapes)
                {
                    if (dictionary.TryGetValue(item.TagShape, out var factory))
                    {
                        var shape = factory.CreateShape(item.TopLeft, item.DownRight, item.BackgroundColor, item.PenColor, item.Angle);
                        shape.StrokeThickness = item.StrokeThickness;

                        abstractShapes.Add(shape);
                        shape.DrawAlgorithm();

                    }
                }

                MessageBox.Show("Список фигур успешно загружен!");
                return (true, abstractShapes);
            }
        }
        return (false, abstractShapes);
    }
}