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

        private BigInteger a = new BigInteger(11);
        private BigInteger b = new BigInteger(22);

        private void button1_Click(object sender, EventArgs e)
        {
            Generate();
            Calculate();
        }

        private void Generate()
        {
            int n = (int)numericUpDown1.Value;
            string astr = "";
            string bstr = "";
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                astr += (r.Next() % 9 + 1).ToString();
                bstr += (r.Next() % 9 + 1).ToString();
            }
            a = BigInteger.Parse(astr);
            b = BigInteger.Parse(bstr);
            textBox2.Text = b.ToString();
            textBox1.Text = a.ToString();
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
            res = BigInteger.ModPow(a, 1, 2);
            textBox6.Text = res.ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Generate();
            Calculate();
        }
    }
}
