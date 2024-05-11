using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TI_2;

public partial class MainForm : Form
{
    readonly StreamCipher streamCipher = new();
    public MainForm()
    {
        InitializeComponent();
    }

    void RegisterTextBox_TextChanged(object sender, EventArgs e)
    {
        LengthLabel.Text = $@"Длина введенных состояний: {RegisterTextBox.Text.Count(x => x is '0' or '1')}";
    }

    void ResultButton_Click(object sender, EventArgs e)
    {
        RegisterTextBox.Text = string.Join("", RegisterTextBox.Text.Where(x => x is '0' or '1'));
        if (RegisterTextBox.Text.Length != 29)
        {
            MessageBox.Show("Длина вашего регистра должна равняться 29 состояниям!", "Внимание");
            return;
        }

        if (PlainTextBox.Text.Length is 0)
        {
            MessageBox.Show("Выберите файл с вашим исходным текстом для шифрования/дешифрования!", "Внимание");
            return;
        }
        
        streamCipher.ProduceBitRegister(RegisterTextBox.Text);
        streamCipher.ProduceBitKey(streamCipher.PlainText.Length);
        KeyTextBox.Text = BitArrayToStr(streamCipher.BitKey);

        streamCipher.Cipher();
        CipherTextBox.Text = BitArrayToStr(streamCipher.CipherBit);
    }

    string BitArrayToStr(BitArray array)
    {
        StringBuilder temp = new();
        if (array.Length <= 240)
        {
            foreach (bool bit in array)
            {
                temp.Append(bit ? 1 : 0);
            }       
        }
        else
        {
            temp.Append("Первые 15 байт: \n");
            for (int i = 0; i < 120; i++)
                temp.Append(array[i] ? 1 : 0);
            temp.Append($"{Environment.NewLine}Последние 15 байт: \n");
            for (int i = 120; i > 0; i--)
            {
                temp.Append(array[array.Length - i] ? 1 : 0);
            }
        }

        return temp.ToString();
    }
    
    void OpenFile_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            var bytes = File.ReadAllBytes(OpenFileDialog.FileName);
            for (int i = 0; i < bytes.Length; i++)
            {
                BitArray help = new BitArray(new[] {bytes[i]});
                foreach (bool bit in help)
                {
                    stringBuilder.Append(bit ? 1 : 0);
                }
            }
            
            /*using FileStream fileStream = new FileStream(OpenFileDialog.FileName, FileMode.Open);
            
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (bytesRead < 1024)
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        byte[] help_arr = { buffer[i] };
                        BitArray help = new BitArray(help_arr);
                        foreach (bool bit in help)
                        {
                            stringBuilder.Append(bit ? 1 : 0);
                        }
                    }
                }
                else
                {
                    BitArray help = new BitArray(buffer);
                    foreach (bool bit in help)
                    {
                        stringBuilder.Append(bit ? 1 : 0);
                    }
                }
            }
            */

            streamCipher.PlainText = new BitArray(stringBuilder.Length);
            for (int i = 0; i < streamCipher.PlainText.Length; i++)
            {
                streamCipher.PlainText[i] = stringBuilder[i] == '1';
            }

            PlainTextBox.Text = BitArrayToStr(streamCipher.PlainText);
        }
    }

    void SaveFile_Click(object sender, EventArgs e)
    {
        if (SaveFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            using FileStream fileStream = new FileStream(SaveFileDialog.FileName, FileMode.Create);
            byte[] result = new byte[streamCipher.CipherBit.Count / 8];
            streamCipher.CipherBit.CopyTo(result, 0);
            fileStream.Write(result, 0, result.Length);
        }
    }

    private void MenuClear_Click(object sender, EventArgs e)
    {
        KeyTextBox.Clear();
        CipherTextBox.Clear();
        PlainTextBox.Clear();
    }

}