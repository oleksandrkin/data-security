using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DES_lab8
{
    public partial class Form1 : Form
    {
        private DES des;

        public Form1()
        {
            InitializeComponent();
            des = new DES();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string a = des.Encript("avbjdggfslgdhs");
            string b = des.Decript();
        }
    }
}
