using System;
using System.Windows.Forms;
using System.Numerics;

namespace BigNumber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private BigInteger a;
        private BigInteger b;
        private Random r = new Random();
        private void button1_Click(object sender, EventArgs e)
        {
            a = GeneratePrime();
            b = GeneratePrime();
            textBox1.Text = a.ToString();
            textBox2.Text = b.ToString();
            Calculate();
        }

        private BigInteger GeneratePrime()
        {
            BigInteger t;
            do
            {
                t = Generate();
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

        private BigInteger Generate()
        {
            int n = (int)numericUpDown1.Value;
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
                    str += (r.Next()%9 + 1).ToString();
                }
                t = BigInteger.Parse(str);
            } while (t < 2 || t > cur);
            return t;
        }

        private void Calculate()
        {
            BigInteger res = new BigInteger();
            res = a + b;
            textBox3.Text = res.ToString();
            res = a * b;
            textBox4.Text = res.ToString();
            res = BigInteger.Pow(a, 2);
            textBox5.Text = res.ToString();
            res = BigInteger.ModPow(a, 1, b);
            textBox6.Text = res.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            a = GeneratePrime();
            b = GeneratePrime();
            textBox1.Text = a.ToString();
            textBox2.Text = b.ToString();
            Calculate();
        }
    }
}
