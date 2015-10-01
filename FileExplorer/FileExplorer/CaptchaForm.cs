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
    public partial class CaptchaForm : Form
    {
        private MainForm mainForm;

        private AccessManager accessManager;
        private int captchaValue;

        public CaptchaForm(MainForm main, AccessManager aManager)
        {
            InitializeComponent();
            mainForm = main;
            accessManager = aManager;
            captchaLabel.Text = CaptchaGenerator.Generate(out captchaValue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            accessManager.Close();
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (captchaAnswerTextBox.Text == "") return;

            foreach (var ch in captchaAnswerTextBox.Text)
            {
                if (Char.IsDigit(ch))
                {
                }
                else
                {
                    MessageBox.Show(@"Captcha must be a number. Try again!");
                    captchaAnswerTextBox.Clear();
                    return;
                }
            }

            if (Convert.ToInt32(captchaAnswerTextBox.Text) == captchaValue)
            {
                mainForm.Enabled = true;
                this.Close();
            }
            else
            {
                accessManager.AddToActivityLog(accessManager.CurrentUser, String.Format("wrong captcha. Exit program."));
                accessManager.Close();
                Application.Exit();
            }
        }
    }
}
