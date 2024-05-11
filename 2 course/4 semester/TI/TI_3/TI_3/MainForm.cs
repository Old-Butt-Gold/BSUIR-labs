using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TI_3;

public partial class MainForm : Form
{
    int IntegerR { get; set; }
    int IntegerFunctionR { get; set; }
    int IntegerE { get; set; }
    int IntegerD { get; set; }

    byte[] OpenedPlainFileBytes { get; set; }
    
    byte[] OpenedCipherFileBytes { get; set; }

    

    ushort[] CipherResult { get; set; }

    byte[] DecipherResult { get; set; }

    
    
    public MainForm()
    {
        InitializeComponent();
    }

    void ButtonR_Click(object sender, EventArgs e)
    {
        TextBoxP.Text = string.Join("", TextBoxP.Text.Where(char.IsDigit));
        TextBoxQ.Text = string.Join("", TextBoxQ.Text.Where(char.IsDigit));
        TextBoxD.Text = string.Join("", TextBoxD.Text.Where(char.IsDigit));

        if (TextBoxP.Text.Length == 0)
        {
            MessageBox.Show("Длина вашего P должна быть отлична от нуля!", "Внимание");
            return;
        }
        
        if (TextBoxQ.Text.Length == 0)
        {
            MessageBox.Show("Длина вашего Q должна быть отлична от нуля!", "Внимание");
            return;
        }

        int IntegerP = 0;
        int IntegerQ = 0;
        try
        {

            IntegerP = int.Parse(TextBoxP.Text);
            if (!RSA.IsPrime(IntegerP))
            {
                MessageBox.Show("Ваше число P не является простым!", "Внимание");
                return;
            }

            IntegerQ = int.Parse(TextBoxQ.Text);
            if (!RSA.IsPrime(IntegerQ))
            {
                MessageBox.Show("Ваше число Q не является простым!", "Внимание");
                return;
            }
        }
        catch
        {
            MessageBox.Show("Проверьте диапазон чисел");
            return;
        }

        IntegerR = IntegerQ * IntegerP;
        if (IntegerR is < 256 or > ushort.MaxValue)
        {
            MessageBox.Show($"Ваше произведение P и Q должно быть не меньше 256 и не больше {ushort.MaxValue}!", "Внимание");
            return;
        }
        
        TextBoxR.Text = IntegerR.ToString();
        IntegerFunctionR = RSA.EulerPhi(IntegerR);
        TextBoxEuler.Text = IntegerFunctionR.ToString();

        if (TextBoxD.Text.Length == 0)
        {
            MessageBox.Show("Длина вашей закрытой константы D должна быть отлична от нуля!", "Внимание");
            return;
        }
        
        IntegerD = int.Parse(TextBoxD.Text);
        if (IntegerD <= 1 || IntegerD >= IntegerFunctionR)
        {
            MessageBox.Show("Ваша закрытая константа D меньше 1 или больше функции эйлера!", "Внимание");
            return;
        }
        
        int gcd = RSA.FindGcd(IntegerD, IntegerFunctionR);
        if (gcd != 1)
        {
            MessageBox.Show("Ваша открытая константа E не взаимно простая с функцией Эйлера!", "Внимание");
            return;
        }

        var extendedEuclidResult = RSA.ExtendedEuclidean(IntegerFunctionR, IntegerD);
        
        IntegerE = extendedEuclidResult.y;

        TextBoxE.Text = IntegerE.ToString();

        ResultButton.Enabled = true;
    }

    void ClearStrip_Click(object sender, EventArgs e)
    {
        TextBoxR.Clear();
        TextBoxEuler.Clear();
        TextBoxE.Clear();
        PlainText.Clear();
        CipherText.Clear();
        ResultButton.Enabled = false;
    }

    void RadioButtonCipher_CheckedChanged(object sender, EventArgs e)
    {
        OpenFileCipher.Enabled = false;
        OpenFilePlain.Enabled = true;
        SaveFileCipher.Enabled = true;
        SaveFilePlain.Enabled = false;
        ResultButton.Text = "Зашифровать";
        CipherText.Clear();
    }

    void RadioButtonDecipher_CheckedChanged(object sender, EventArgs e)
    {
        OpenFileCipher.Enabled = true;
        OpenFilePlain.Enabled = false;
        SaveFileCipher.Enabled = false;
        SaveFilePlain.Enabled = true;
        ResultButton.Text = "Дешифровать";
        PlainText.Clear();
    }

    void ResultButton_Click(object sender, EventArgs e)
    {
        if (RadioButtonCipher.Checked)
        {
            if (PlainText.Text.Length == 0)
            {
                MessageBox.Show("Длина вашего исходного текста должна быть отлична от нуля. Попробуйте открыть файл!", "Внимание");
                return;
            }

            CipherResult = new ushort[OpenedPlainFileBytes.Length];
            for (int i = 0; i < CipherResult.Length; i++)
            {
                CipherResult[i] = OpenedPlainFileBytes[i];
            }
            
            for (int i = 0; i < CipherResult.Length; i++)
            {
                CipherResult[i] = (ushort)RSA.QuickPowerMod(CipherResult[i], IntegerE, IntegerR);
            }

            CipherText.Text = string.Join(" ", CipherResult);
        }

        if (RadioButtonDecipher.Checked)
        {
            if (CipherText.Text.Length == 0)
            {
                MessageBox.Show("Длина вашего зашифрованного текста должна быть отлична от нуля. Попробуйте открыть файл!",
                    "Внимание");
                return;
            }

            ushort[] tempShort = new ushort[CipherResult.Length];
            
            for (int i = 0; i < tempShort.Length; i++)
            {
                tempShort[i] = (ushort)RSA.QuickPowerMod(CipherResult[i], IntegerD, IntegerR);
            }

            DecipherResult = new byte[tempShort.Length];
            for (var index = 0; index < tempShort.Length; index++)
            {
                var item = tempShort[index];
                var bytes = BitConverter.GetBytes(item);
                if (!BitConverter.IsLittleEndian) 
                    Array.Reverse(bytes);
                DecipherResult[index] = bytes[0];

            }

            PlainText.Text = string.Join(" ", tempShort);
        }
    }

    void OpenFilePlain_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            OpenedPlainFileBytes = File.ReadAllBytes(OpenFileDialog.FileName);
            PlainText.Text = string.Join(" ", OpenedPlainFileBytes);
        }
    }

    void SaveFileCipher_Click(object sender, EventArgs e)
    {
        if (CipherText.Text.Length == 0)
        {
            MessageBox.Show("Нечего сохранять!", "Внимание");
            return;
        }
        if (SaveFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            using FileStream fileStream = new FileStream(SaveFileDialog.FileName, FileMode.Create);
            foreach (var item in CipherResult)
            {
                byte[] bytes = BitConverter.GetBytes(item);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }

    void OpenFileCipher_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            OpenedCipherFileBytes = File.ReadAllBytes(OpenFileDialog.FileName);

            //Если в каком-то рандомном файле не кратное двум число байт
            if (OpenedCipherFileBytes.Length % 2 != 0)
            {
                OpenedCipherFileBytes = OpenedCipherFileBytes.Append((byte) 0).ToArray();
            }
            
            CipherResult = new ushort[OpenedCipherFileBytes.Length / 2];
            
            for (int i = 0; i < OpenedCipherFileBytes.Length; i += 2)
            {
                byte[] bytes = [OpenedCipherFileBytes[i], OpenedCipherFileBytes[i + 1]];
                ushort combinedShort = BitConverter.ToUInt16(bytes, 0);
                CipherResult[i / 2] = combinedShort;
            }
            
            CipherText.Text = string.Join(" ", CipherResult);
        }
    }

    private void SaveFilePlain_Click(object sender, EventArgs e)
    {
        if (PlainText.Text.Length == 0)
        {
            MessageBox.Show("Нечего сохранять!", "Внимание");
            return;
        }
        
        if (SaveFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            File.WriteAllBytes(SaveFileDialog.FileName, DecipherResult);
        }
    }
}