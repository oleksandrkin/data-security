using System;
using System.Numerics;

namespace lab7_RSA
{
    public class RSA
    {
        private BigInteger p;
	    private BigInteger q;
	    private BigInteger N;
	    private BigInteger m;
	    private BigInteger e;
	    private BigInteger d;

	    private Random r = new Random();
        private int keyLength = 100;

	    public RSA() 
        {
	    	r = new Random();
            p = GeneratePrime(keyLength);
            q = GeneratePrime(keyLength);
	    	N = p * q;
	        m = (p - 1)*(q - 1);
            d = GeneratePrime(keyLength);
	    	while ( BigInteger.GreatestCommonDivisor(m, d) > 1 && d < m) 
            {
	    		d++;
	    	}
	    	e = ModInverse(d, m);
	    }

        private BigInteger GeneratePrime(int l)
        {
            BigInteger t;
            do
            {
                t = Generate(l);
            } while (!MillerRabinTest(t, 100));
            return t;
        }

        private bool MillerRabinTest(BigInteger n, int k)
        {
            if (n <= 1)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            BigInteger s = 0, d = n - 1;
            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger t = Generate(n.ToByteArray().Length, n);
                BigInteger x = BigInteger.ModPow(t, d, n);
                if (x == 1 || x == n - 1)
                    continue;
                for (int j = 0; j < s - 1; j++)
                {
                    x = (x * x) % n;
                    if (x == 1)
                        return false;
                    if (x == n - 1)
                        break;
                }
                if (x != n - 1)
                    return false;
            }
            return true;
        }

        private BigInteger Generate(int n)
        {
            string str = "";
            for (int i = 0; i < n; i++)
            {
                str += (r.Next() % 9 + 1).ToString();
            }
            return BigInteger.Parse(str);
        }

        private BigInteger Generate(int m, BigInteger cur)
        {
            int n = r.Next(m - 1) + 1;
            BigInteger t = new BigInteger();
            do
            {
                string str = "";
                for (int i = 0; i < n; i++)
                {
                    str += (r.Next() % 9 + 1).ToString();
                }
                t = BigInteger.Parse(str);
            } while (t < 2 || t > cur);
            return t;
        }

        BigInteger ModInverse(BigInteger a, BigInteger n)
        {
            BigInteger i = n, v = 0, d = 1;
            while (a > 0)
            {
                BigInteger t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }

	    public string BytesToString(byte[] array) 
        {
	    	string test = "";
	    	foreach (byte b in array) 
            {
	    		test += b.ToString();
	    	}
	    	return test;
	    }

	    public byte[] Encrypt(byte[] message)
	    {
	        return BigInteger.ModPow(new BigInteger(message), e, N).ToByteArray();
	    }

	    public byte[] Decrypt(byte[] message) 
        {
            return BigInteger.ModPow(new BigInteger(message), d, N).ToByteArray();
	    }
    }
}
