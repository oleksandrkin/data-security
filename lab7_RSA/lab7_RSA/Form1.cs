using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab7_RSA
{
    public partial class Form1 : Form
    {
        private RSA rsa;
        private byte[] encrypted;

        public Form1()
        {
            InitializeComponent();
            rsa = new RSA();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            encrypted = rsa.Encrypt(Encoding.ASCII.GetBytes(textBox1.Text));
            textBox2.Text = rsa.BytesToString(encrypted);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox4.Text = Encoding.ASCII.GetString(rsa.Decrypt(encrypted));
        }
    }
}
