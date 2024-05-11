using System.Text;
using Bogus;

public class TaskManager
{
    string[] morseCodes = { ".-", "-...", "-.-.", "-..", ".", "..-.", "--.", "....", "..", ".---", "-.-", ".-..",
        "--", "-.", "---", ".--.", "--.-", ".-.", "...", "-", "..-", "...-", ".--", "-..-", "-.--", "--..", // буквы
        "-----", ".----", "..---", "...--", "....-", ".....", "-....", "--...", "---..", "----.", // цифры
        ".-.-.-", "--..--", "..--..", "..--.", "---...", ".-..-.", ".----.", "-...-", 
        "-.--.", "-.--.-", ".-...", "-.-.-.", "..--.-", ".-.-.", "-....-", "...-..-", ".--.-."
    };
    
    char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,?!:\"\'=()&;_+-$@".ToLower().ToCharArray();

    Dictionary<char, string> textDict;

    Dictionary<string, char> morseDict;
    
    public TaskManager()
    {
        
        textDict = new();
        for (int i = 0; i < alphabet.Length; i++)
            textDict.Add(alphabet[i], morseCodes[i]);
        
        morseDict = new();
        for (int i = 0; i < alphabet.Length; i++)
            morseDict.Add(morseCodes[i], alphabet[i]);
        
    }

    public string TextToMorseCode(string text)
    {
        text = text.ToLower();

        StringBuilder morseCodeBuilder = new StringBuilder();

        foreach (char character in text)
            morseCodeBuilder.Append(character == ' ' ? "|   " : $"{textDict[character]}   ");

        return morseCodeBuilder.ToString().Trim(); // Убираем лишний пробел в конце, если есть
    }
    
    public string MorseCodeToText(string morseCode)
    {
        string[] words = morseCode.Split(" ", StringSplitOptions.RemoveEmptyEntries);
        
        StringBuilder decodedText = new();
        foreach (string word in words)
            decodedText.Append(morseDict.TryGetValue(word, out var value) ? value : " ");

        return decodedText.ToString();
    }
}