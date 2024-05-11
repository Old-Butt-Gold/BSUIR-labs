using System;
using System.Numerics;

namespace TI_4;

static class EDS
{
    static readonly BigInteger H0 = 100;

    public static BigInteger HashFunction(byte[] textBytes, BigInteger bigIntegerR)
    {
        if (textBytes == null || textBytes.Length == 0)
            throw new Exception("Check your textBytes array");
        
        BigInteger result = RSA.QuickPowerMod(H0 + new BigInteger(textBytes[0]), 2, bigIntegerR);
        
        for (int i = 1; i < textBytes.Length; i++)
        {
            result = RSA.QuickPowerMod(result + new BigInteger(textBytes[i]), 2, bigIntegerR);
        }
        
        return result;
    }
}