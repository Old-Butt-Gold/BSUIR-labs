using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TI_1
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        void ClearMenuStripItem_Click(object sender, EventArgs e)
        {
            PlainTextBox.Clear();
            KeyTextBox.Clear();
            ResultTextBox.Clear();
        }

        void CalculateButton_Click(object sender, EventArgs e)
        {
            if (RailwayFenceRadioButton.Checked)
            {
                int key = RailwayFence.GetKey(KeyTextBox.Text);
                if (key == -1)
                {
                    MessageBox.Show("Проверьте ваш ключ, чтобы он содержал цифры", "Неправильный ключ");
                    return;
                }

                KeyTextBox.Text = key.ToString();
                string plainText = RailwayFence.GetPlainText(PlainTextBox.Text);
                Func<string, int, string> processFunction =
                    EncipherRadioButton.Checked ? RailwayFence.Encipher : RailwayFence.Decipher;
                
                string cipher = processFunction(plainText, key);
                if (cipher != plainText)
                {
                    ResultTextBox.Text = cipher;
                }
                
            }

            if (VizhinerRadioButton.Checked)
            {
                string key = Vigener.GetPlainTextOrKey(KeyTextBox.Text);
                if (key is "")
                {
                    MessageBox.Show("Проверьте ваш ключ, чтобы он содержал русские буквы", "Неправильный ключ");
                    return;
                }
                
                KeyTextBox.Text = key;
                
                Func<string, string, string> processFunction =
                    EncipherRadioButton.Checked ? Vigener.Encipher : Vigener.Decipher;
                var result = processFunction(PlainTextBox.Text, key);
                
                if (result == "")
                {
                    MessageBox.Show("Проверьте ваш вводимый текст, чтобы он содержал русские буквы", "Неправильный ключ");
                    return;
                }
                ResultTextBox.Text = result;
                
            }
        }

        void PlainTextBox_TextChanged(object sender, EventArgs e)
        {
            ResultTextBox.Clear();
        }

        void SaveFileMenu_Click(object sender, EventArgs e)
        {
            if (ResultTextBox.Text.Length is 0)
            {
                MessageBox.Show("Нет результатов для сохранения", "Внимание");
                return;
            }
            
            var dialogResult = SaveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                using StreamWriter sw = new StreamWriter(SaveFileDialog.FileName);
                sw.WriteLine(ResultTextBox.Text);
            }
        }

        void OpenFileMenu_Click(object sender, EventArgs e)
        {
            var dialogResult = OpenFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                using StreamReader sw = new StreamReader(OpenFileDialog.FileName);
                StringBuilder sb = new StringBuilder();
                var str = sw.ReadToEnd().Where(x => x != '\n');
                foreach (var item in str)
                {
                    sb.Append(item);
                }
                PlainTextBox.Text = sb.ToString();
            }
        }
    }
}