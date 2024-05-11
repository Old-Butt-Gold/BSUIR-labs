using System.Collections;

namespace TI_2;

public class StreamCipher
{
    public BitArray BitRegister { get; private set; }
    public BitArray BitKey { get; private set; }
    public BitArray PlainText { get; set; }
    public BitArray CipherBit { get; private set; }
    
    public void ProduceBitRegister(string parsingString)
    {
        BitRegister = new BitArray(parsingString.Length);
        for (int i = 0; i < parsingString.Length; i++)
            BitRegister[i] = parsingString[i] == '1';
    }

    public void ProduceBitKey(int length)
    {
        BitKey = new(length);
        for (int i = 0; i < length; i++)
        {
            BitKey[i] = BitRegister[0];
            
            //Полином для регистра длиной 29 бита x^29 + x^2 + 1
            int len = BitRegister.Length;
            bool nextValue = BitRegister[len - 1 - 28] ^ BitRegister[len - 1 - 1];

            for (int index = 0; index < BitRegister.Length - 1; index++)
            {
                BitRegister[index] = BitRegister[index + 1];
            }
            
            BitRegister[BitRegister.Length - 1] = nextValue;
        }
    }

    public void Cipher() => CipherBit = BitKey.Xor(PlainText); //Именно так, а не наоборот!
}
