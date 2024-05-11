using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace TI_3;

public partial class MainForm : Form
{
    const int CountOfTests = 10;

    BigInteger BigIntegerR { get; set; }
    BigInteger BigIntegerFunctionR { get; set; }
    BigInteger BigIntegerE { get; set; }
    BigInteger BigIntegerD { get; set; }

    byte[] OpenedFileBytes { get; set; }

    BigInteger[] CipherResult { get; set; }

    List<BigInteger> CipherFileBigIntegerList { get; set; }

    BigInteger[] PlainBigIntegerArray { get; set; }
    
    public MainForm() => InitializeComponent();

    private void ButtonR_Click(object sender, EventArgs e)
    {
        TextBoxP.Text = string.Join("", TextBoxP.Text.Where(char.IsDigit));
        TextBoxQ.Text = string.Join("", TextBoxQ.Text.Where(char.IsDigit));
        TextBoxE.Text = string.Join("", TextBoxE.Text.Where(char.IsDigit));

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

        BigInteger bigIntegerP = BigInteger.Parse(TextBoxP.Text);
        if (!RSA.IsPrime(bigIntegerP, CountOfTests))
        {
            MessageBox.Show("Ваше число P не является простым!", "Внимание");
            return;
        }
        
        BigInteger bigIntegerQ = BigInteger.Parse(TextBoxQ.Text);
        if (!RSA.IsPrime(bigIntegerQ, CountOfTests))
        {
            MessageBox.Show("Ваше число Q не является простым!", "Внимание");
            return;
        }

        BigIntegerR = bigIntegerQ * bigIntegerP;
        if (BigIntegerR < 256)
        {
            MessageBox.Show("Ваше произведение P и Q должно быть не меньше 256!", "Внимание");
            return;
        }
        
        TextBoxR.Text = BigIntegerR.ToString();
        BigIntegerFunctionR = RSA.EulerPhi(BigIntegerR);
        TextBoxEuler.Text = BigIntegerFunctionR.ToString();

        if (TextBoxE.Text.Length == 0)
        {
            MessageBox.Show("Длина вашей открытой константы E должна быть отлична от нуля!", "Внимание");
            return;
        }
        
        BigIntegerE = BigInteger.Parse(TextBoxE.Text);
        if (BigIntegerE < 1 || BigIntegerE >= BigIntegerFunctionR)
        {
            MessageBox.Show("Ваша открытая константа E меньше 1 или больше функции эйлера!", "Внимание");
            return;
        }
        
        BigInteger gcd = RSA.FindGcd(BigIntegerE, BigIntegerFunctionR);
        if (gcd != 1)
        {
            MessageBox.Show("Ваша открытая константа E не взаимно простая с функцией Эйлера!", "Внимание");
            return;
        }
        
        var extendedEuclidResult = BigIntegerFunctionR > BigIntegerE 
            ? RSA.EuclidExtended(BigIntegerFunctionR, BigIntegerE) 
            : RSA.EuclidExtended(BigIntegerE, BigIntegerFunctionR);
        
        BigIntegerD = extendedEuclidResult.y1;

        if (BigIntegerD < 0)
        {
            BigIntegerD += BigIntegerFunctionR;
        }

        TextBoxD.Text = BigIntegerD.ToString();

        ResultButton.Enabled = true;
    }

    void ClearStrip_Click(object sender, EventArgs e)
    {
        TextBoxR.Clear();
        TextBoxEuler.Clear();
        TextBoxD.Clear();
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
                MessageBox.Show("Длина вашего исходного текста должна быть отлична от нуля. Попробуйте открыть файл!",
                    "Внимание");
                return;
            }

            CipherResult = new BigInteger[OpenedFileBytes.Length];
            for (int i = 0; i < CipherResult.Length; i++)
            {
                CipherResult[i] = new(OpenedFileBytes[i]);
            }
            
            for (int i = 0; i < CipherResult.Length; i++)
            {
                CipherResult[i] = RSA.QuickPowerMod(CipherResult[i], BigIntegerE, BigIntegerR);
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

            PlainBigIntegerArray = CipherFileBigIntegerList.ToArray();
            
            for (int i = 0; i < PlainBigIntegerArray.Length; i++)
            {
                PlainBigIntegerArray[i] = RSA.QuickPowerMod(PlainBigIntegerArray[i], BigIntegerD, BigIntegerR);
            }

            PlainText.Text = string.Join(" ", PlainBigIntegerArray);
        }
    }

    void OpenFilePlain_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            OpenedFileBytes = File.ReadAllBytes(OpenFileDialog.FileName);
            PlainText.Text = string.Join(" ", OpenedFileBytes);
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
            using BinaryWriter binaryWriter = new BinaryWriter(new FileStream(SaveFileDialog.FileName, FileMode.Create));
            foreach (var number in CipherResult)
            {
                binaryWriter.Write(number.ToString());
            }
            binaryWriter.Flush();
        }
    }

    void OpenFileCipher_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            CipherFileBigIntegerList = new List<BigInteger>();
            using BinaryReader binaryReader = new BinaryReader(new FileStream(OpenFileDialog.FileName, FileMode.Open));
            while (binaryReader.PeekChar() > -1)
            {
                if (BigInteger.TryParse(binaryReader.ReadString(), out var number))
                {
                    CipherFileBigIntegerList.Add(number);
                }
            }

            CipherText.Text = string.Join(" ", CipherFileBigIntegerList);
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
            List<byte> list = new();
            foreach (var item in PlainBigIntegerArray)
            {
                list.Add(item.ToByteArray()[0]);
            }
            File.WriteAllBytes(SaveFileDialog.FileName, list.ToArray());
        }
    }
}