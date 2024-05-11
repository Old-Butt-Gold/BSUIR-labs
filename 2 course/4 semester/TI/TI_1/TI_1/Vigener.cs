using System.Text;

namespace TI_1;

public static class Vigener
{
    public const int LetterCount = 33;
    public static string GetPlainTextOrKey(string str)
    {
        StringBuilder sb = new();
        foreach (char symbol in str)
        {
            var upperSymbol = char.ToUpper(symbol);
            if (upperSymbol is <= 'Я' and >= 'А' or 'Ё')
                sb.Append(upperSymbol);
        }
        return sb.ToString();
    }

    private static string GetPlainTextWithSpaces(string str)
    {
        StringBuilder sb = new();
        foreach (char symbol in str)
        {
            var upperSymbol = char.ToUpper(symbol);
            if (upperSymbol is <= 'Я' and >= 'А' or 'Ё' or ' ')
                sb.Append(upperSymbol);
        }
        return sb.ToString();
    }

    public static string Encipher(string plainText, string key)
    {
        plainText = GetPlainTextWithSpaces(plainText);
        var resultText = GetPlainTextOrKey(plainText);
        if (resultText is "") return "";
        
        char[] letterArray = new char[LetterCount];
        int letter = 0;
        for (char i = 'А'; i <= 'Я'; i++)
        {
            if (letter == 6)
            {
                letterArray[letter++] = 'Ё';
            }
            letterArray[letter++] = i;
        }

        StringBuilder sbCipherText = new StringBuilder();
        int index = 0;
        for (int i = 0; i < resultText.Length; i++)
        {
            char keyLetter = key[index % key.Length];
            int changedLetter;
            
            if (resultText[i] == 'Ё')
            {
                changedLetter = 6;
            } 
            else
                changedLetter = resultText[i] <= 'Е' ? resultText[i] - 'А' : resultText[i] - 'А' + 1;

            
            switch (keyLetter)
            {
                case 'Ё':
                    sbCipherText.Append(letterArray[(changedLetter + 6) % LetterCount]);
                    break;
                case <= 'Е':
                {
                    int changedKeyLetter = keyLetter - 'А';
                    sbCipherText.Append(letterArray[(changedLetter + changedKeyLetter) % LetterCount]);
                    break;
                }
                //Из-за буквы Ё
                default:
                {
                    int changedKeyLetter = keyLetter - 'А' + 1;
                    sbCipherText.Append(letterArray[(changedLetter + changedKeyLetter) % LetterCount]); //+1 из-за буквы Ё
                    break;
                }
            }

            index++;
        }
        
        StringBuilder sbResultText = new StringBuilder(plainText);
        index = 0;
        for (int i = 0; i < sbResultText.Length; i++)
        {
            if (sbResultText[i] == ' ') continue;
            sbResultText[i] = sbCipherText[index++];
        }
        
        return sbResultText.ToString();
    }

    public static string Decipher(string cipher, string key)
    {
        cipher = GetPlainTextWithSpaces(cipher);
        var resultText = GetPlainTextOrKey(cipher);
        if (resultText is "")
        {
            return "";
        }
        
        char[] letterArray = new char[LetterCount];
        int letter = 0;
        for (char i = 'А'; i <= 'Я'; i++)
        {
            if (letter == 6)
            {
                letterArray[letter++] = 'Ё';
            }
            letterArray[letter++] = i;
        }
        
        StringBuilder sbPlainText = new StringBuilder();
        int index = 0;
        for (int i = 0; i < resultText.Length; i++)
        {
            char keyLetter = key[index % key.Length];
            int changedLetter;
            
            if (resultText[i] == 'Ё')
            {
                changedLetter = 6;
            } 
            else
                changedLetter = resultText[i] <= 'Е' ? resultText[i] - 'А' : resultText[i] - 'А' + 1;

            
            switch (keyLetter)
            {
                case 'Ё':
                    sbPlainText.Append(letterArray[(changedLetter + (LetterCount - 6)) % LetterCount]);
                    break;
                case <= 'Е':
                {
                    int changedKeyLetter = keyLetter - 'А';
                    sbPlainText.Append(letterArray[(changedLetter + (LetterCount - changedKeyLetter)) % LetterCount]);
                    break;
                }
                //Из-за буквы Ё
                default:
                {
                    int changedKeyLetter = keyLetter - 'А' + 1;
                    sbPlainText.Append(letterArray[(changedLetter + (LetterCount - changedKeyLetter)) % LetterCount]); //+1 из-за буквы Ё
                    break;
                }
            }
            index++;
        }    
        
        StringBuilder sbResultText = new StringBuilder(cipher);
        index = 0;
        for (int i = 0; i < sbResultText.Length; i++)
        {
            if (sbResultText[i] == ' ') continue;
            sbResultText[i] = sbPlainText[index++];
        }

        return sbResultText.ToString();
    }
}