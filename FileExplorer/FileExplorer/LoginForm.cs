using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class LoginForm : Form
    {
        private AccessManager accessManager;
        private Form parentForm;
        private int captchaValue;

        public LoginForm(AccessManager am, Form sender)
        {
            InitializeComponent();
            accessManager = am;
            parentForm = sender;
            captchaLabel.Text  = CaptchaGenerator.Generate(out captchaValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO !!
            //parentForm.Show();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = ((CheckBox)sender).Checked ? '\0' : '#';
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            captchaLabel.Text = CaptchaGenerator.Generate(out captchaValue);
        }
    }
}
