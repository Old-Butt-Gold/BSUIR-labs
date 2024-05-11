using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Forms;

namespace TI_4;

public partial class MainForm : Form
{
    const int CountOfTests = 10;

    BigInteger BigIntegerR { get; set; }
    BigInteger BigIntegerFunctionR { get; set; }
    BigInteger BigIntegerE { get; set; }
    BigInteger BigIntegerD { get; set; }

    byte[] OpenedFileBytes { get; set; }

    BigInteger HashFunction { get; set; }
    
    BigInteger EdsValue { get; set; }

    public MainForm() => InitializeComponent();

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

        if (TextBoxD.Text.Length == 0)
        {
            MessageBox.Show("Длина вашей закрытой константы D должна быть отлична от нуля!", "Внимание");
            return;
        }
        
        BigIntegerD = BigInteger.Parse(TextBoxD.Text);
        if (BigIntegerD < 1 || BigIntegerD >= BigIntegerFunctionR)
        {
            MessageBox.Show("Ваша закрытая константа D меньше 1 или больше функции эйлера!", "Внимание");
            return;
        }
        
        BigInteger gcd = RSA.FindGcd(BigIntegerD, BigIntegerFunctionR);
        if (gcd != 1)
        {
            MessageBox.Show("Ваша закрытая константа D не взаимно простая с функцией Эйлера!", "Внимание");
            return;
        }
        
        var extendedEuclidResult = BigIntegerFunctionR > BigIntegerD
            ? RSA.EuclidExtended(BigIntegerFunctionR, BigIntegerD) 
            : RSA.EuclidExtended(BigIntegerD, BigIntegerFunctionR);
        
        BigIntegerE = extendedEuclidResult.y1;

        if (BigIntegerE < 0)
        {
            BigIntegerE += BigIntegerFunctionR;
        }

        TextBoxE.Text = BigIntegerE.ToString();

        ResultButton.Enabled = true;
    }

    void ClearStrip_Click(object sender, EventArgs e)
    {
        TextBoxR.Clear();
        TextBoxEuler.Clear();
        TextBoxE.Clear();
        PlainText.Clear();
        ResultButton.Enabled = false;
        TextBoxHash.Clear();
        TextBoxEDS.Clear();
    }

    void ResultButton_Click(object sender, EventArgs e)
    {
        if (PlainText.Text.Length == 0)
        {
            MessageBox.Show("Длина вашего текста должна быть отлична от нуля. Попробуйте открыть файл!",
                "Внимание");
            return;
        }
        
        if (RadioButtonCreate.Checked)
        {
            HashFunction = EDS.HashFunction(OpenedFileBytes, BigIntegerR);
            TextBoxHash.Text = HashFunction.ToString();
            EdsValue = RSA.QuickPowerMod(HashFunction, BigIntegerD, BigIntegerR);
            TextBoxEDS.Text = EdsValue.ToString();
            SaveFile.Enabled = true;
        }

        if (RadioButtonCheck.Checked)
        {
            if (TextBoxEDS.Text.Length == 0)
            {
                MessageBox.Show("Сначала вычислите ЭЦП для первоначального текста!", "Внимание");
                return;
            }
            
            HashFunction = EDS.HashFunction(OpenedFileBytes, BigIntegerR);
            TextBoxHash.Text = HashFunction.ToString();
            var temp = RSA.QuickPowerMod(EdsValue, BigIntegerE, BigIntegerR);
            string result = temp != HashFunction ? "Подпись не верна!" : "Подпись верна!";
            MessageBox.Show($"Значение Хэш-образа текста: {HashFunction}{Environment.NewLine}" +
                            $"Значение Хэш-образа по ключу с ЭЦП: {temp}{Environment.NewLine}" +
                            $"{result}");

        }

    }

    void OpenFilePlain_Click(object sender, EventArgs e)
    {
        if (OpenFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            PlainText.Text = File.ReadAllText(OpenFileDialog.FileName);
            OpenedFileBytes = Encoding.UTF8.GetBytes(PlainText.Text);
        }
    }

    void SaveFilePlain_Click(object sender, EventArgs e)
    {
        if (SaveFileDialog.ShowDialog() != DialogResult.Cancel)
        {
            File.WriteAllText(SaveFileDialog.FileName, 
                PlainText.Text + Environment.NewLine + $"Подпись: {TextBoxEDS.Text}");
        }
    }

    void RadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if (RadioButtonCheck.Checked)
            SaveFile.Enabled = false;
        PlainText.Clear();
        TextBoxHash.Clear();
    }
}