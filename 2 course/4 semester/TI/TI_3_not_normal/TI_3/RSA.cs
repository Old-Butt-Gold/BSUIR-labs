using System;
using System.Numerics;

namespace TI_3;

static class RSA
{
    //Вычисляет количество взаимно простых чисел до n 
    public static BigInteger EulerPhi(BigInteger n)
    {
        
        BigInteger result = n; // Изначально присваиваем результату значение n

        // Перебираем все простые числа, которые делят n
        for (BigInteger p = 2; p * p <= n; ++p)
        {
            if (n % p == 0)
            {
                // Если p делит n, уменьшаем result на result / p и на p - 1
                while (n % p == 0)
                {
                    n /= p;
                }
                result -= result / p;
            }
        }

        // Если n осталось простым, уменьшаем result на result / n
        if (n > 1)
        {
            result -= result / n;
        }

        return result;
    }
    
    public static bool IsPrime(BigInteger number, int tests)
    {
        if (number <= 1)
            return false;

        if (number == 2 || number == 3)
            return true;

        if (number.IsEven)
            return false;
        
        // Разложение числа n - 1 в виде (2^s) * d
        BigInteger d = number - 1;
        int s = 0;
        while (d.IsEven)
        {
            d >>= 1;
            s++;
        }

        Random random = new();

        for (int i = 0; i < tests; i++)
        {
            BigInteger witness = GenerateRandomWitness(2, number - 2);
            BigInteger x = QuickPowerMod(witness, d, number);

            if (x == 1 || x == number - 1)
                continue;

            for (int j = 1; j < s; j++)
            {
                x = BigInteger.ModPow(x, 2, number);
                if (x == 1)
                    return false;
                if (x == number - 1)
                    break;
            }

            if (x != number - 1)
                return false;
        }

        return true;
        
        BigInteger GenerateRandomWitness(int minValue, BigInteger maxValue)
        {
            byte[] bytes = maxValue.ToByteArray();
            BigInteger randomWitness;
            do
            {
                random.NextBytes(bytes);
                bytes[bytes.Length - 1] &= 0x7F; // Ensure the number is positive
                randomWitness = new BigInteger(bytes);
            } while (randomWitness >= maxValue || randomWitness < minValue);

            return randomWitness;
        }
    }
    
    public static BigInteger FindGcd(BigInteger a, BigInteger b) => b == 0 ? a : FindGcd(b, a % b);

    public static (BigInteger d, BigInteger x1, BigInteger y1) EuclidExtended(BigInteger a, BigInteger b)
    {
        BigInteger d0 = a;
        BigInteger d1 = b;
        BigInteger x0 = 1;
        BigInteger x1 = 0;
        BigInteger y0 = 0;
        BigInteger y1 = 1;

        while (d1 > 1)
        {
            BigInteger q = d0 / d1;
            BigInteger d2 = d0 % d1;
            BigInteger x2 = x0 - q * x1;
            BigInteger y2 = y0 - q * y1;

            d0 = d1;
            d1 = d2;
            x0 = x1;
            x1 = x2;
            y0 = y1;
            y1 = y2;
        }

        return (d1, x1, y1);
    }
    
    public static BigInteger QuickPowerMod(BigInteger num, BigInteger power, BigInteger mod)
    {
        if (mod == 1)
            return 0;
        
        if (power == 0)
            return 1;

        if (num == 0)
            return 0;

        BigInteger result = 1;
        BigInteger current = num % mod;
        BigInteger exponent = power < 0 ? -power : power;

        while (exponent > 0)
        {
            if (exponent % 2 == 1)
                result = (result * current) % mod;

            current = (current * current) % mod;
            exponent /= 2;
        }

        return power < 0 ? ModInverse(result, mod) : result;
        
        static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0, x = 1;

            while (a > 1)
            {
                BigInteger q = a / m;
                BigInteger t = m;
                m = a % m;
                a = t;
                t = y;
                y = x - q * y;
                x = t;
            }

            if (x < 0)
                x += m0;

            return x;
        }
    }
}