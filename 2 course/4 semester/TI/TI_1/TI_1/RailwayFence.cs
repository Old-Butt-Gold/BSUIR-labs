using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TI_1
{
    public static class RailwayFence
    {
        
        public static int GetKey(string str)
        {
            string text = string.Join("", str.Where(char.IsDigit));
            if (int.TryParse(text, out var value))
            {
                var newValue = Math.Abs(value);
                if (newValue == 0) return -1;
                return value;
            }

            return -1;
        }

        public static string GetPlainText(string str)
        {
            StringBuilder sb = new();
            foreach (char symbol in str)
            {
                var upperSymbol = char.ToUpper(symbol);
                if (upperSymbol is <= 'Z' and >= 'A')
                    sb.Append(upperSymbol);
            }

            return sb.ToString();
        }

        public static string Encipher(string plainText, int key)
        {
            if (plainText.Length is 0)
            {
                MessageBox.Show("Длина вашего текста должна быть отличная от нуля и содержать английские буквы!", "Внимание");
                return plainText;
            }

            if (key >= plainText.Length)
            {
                MessageBox.Show("Длина вашего текста должна быть больше значения ключа", "Внимание");
                return plainText;
            }

            if (key is 1)
            {
                MessageBox.Show("Ваш ключ не должен быть единицей", "Внимание");
                return plainText;
            }

            List<List<char>> fence = new List<List<char>>();
            for (int i = 0; i < key; i++)
            {
                fence.Add(new());
            }
            
            int row = 0;
            int direction = 1;
            for (int i = 0; i < plainText.Length; i++)
            {
                fence[row].Add(plainText[i]);
                row += direction;
                if (row == key - 1 || row == 0)
                    direction *= -1;
            }
            
            StringBuilder cipher = new StringBuilder();
            for (int i = 0; i < key; i++)
            {
                foreach (var ch in fence[i])
                {
                    cipher.Append(ch);
                }
                cipher.Append(" ");
            }

            return cipher.ToString();
        }


        public static string Decipher(string cypherText, int key)
        {
            if (cypherText.Length is 0)
            {
                MessageBox.Show("Длина вашего текста должна быть отличная от нуля и содержать английские буквы!", "Внимание");
                return cypherText;
            }

            if (key >= cypherText.Length)
            {
                MessageBox.Show("Длина вашего текста должна быть больше значения ключа", "Внимание");
                return cypherText;
            }

            if (key is 1)
            {
                MessageBox.Show("Ваш ключ не должен быть единицей", "Внимание");
                return cypherText;
            }

            
            char[,] fence = new char[key, cypherText.Length];
            int row = 0;
            int direction = 1;
            
            for (int i = 0; i < cypherText.Length; i++)
            {
                fence[row, i] = '-';
                row += direction;

                if (row == key - 1 || row == 0)
                    direction *= -1;
            }
            
            int index = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cypherText.Length; j++)
                {
                    if (fence[i, j] == '-' && index < cypherText.Length)
                        fence[i, j] = cypherText[index++];
                }
            }
            
            StringBuilder plainText = new StringBuilder();
            row = 0;
            direction = 1;
            for (int i = 0; i < cypherText.Length; i++)
            {
                plainText.Append(fence[row, i]);
                row += direction;

                if (row == key - 1 || row == 0)
                    direction *= -1;
            }
            
            return plainText.ToString();
        }

    }
    
}