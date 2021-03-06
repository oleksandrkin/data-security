﻿using System;
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
        private readonly AccessManager accessManager;
        private readonly MainForm parentForm;
        private int captchaValue;

        public LoginForm(AccessManager am, MainForm sender)
        {
            InitializeComponent();
            accessManager = am;
            parentForm = sender;
            captchaLabel.Text = CaptchaGenerator.Generate(out captchaValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (captchaAnswerTextBox.Text == ""
                || passwordTextBox.Text == ""
                || loginTextBox.Text == "") return;

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
                if (accessManager.Authorization(loginTextBox.Text, passwordTextBox.Text))
                {
                    this.Close();
                    parentForm.Show();
                }
                else
                {
                    MessageBox.Show(@"Wrong login/password. Try again!");
                    bool control = ++accessManager.CriticalAccess > accessManager.CriticalAccessControl;
                    accessManager.AddToActivityLog(String.Format("Wrong login/password: \"{0}\" and pass: \"{1}\"; Critical number: {2}", loginTextBox.Text, passwordTextBox.Text, accessManager.CriticalAccess), control);
                    loginTextBox.Clear();
                    passwordTextBox.Clear();
                }
            }
            else
            {
                MessageBox.Show(@"Wrong captcha. Try again!");
                bool control = ++accessManager.CriticalCaptcha > accessManager.CriticalCaptchaControl;
                accessManager.AddToActivityLog(String.Format("Wrong captcha with login: \"{0}\" and pass: \"{1}\"; Critical number: {2}", loginTextBox.Text, passwordTextBox.Text, accessManager.CriticalCaptcha), control);
                captchaAnswerTextBox.Clear();
                refreshButton.PerformClick();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = ((CheckBox)sender).Checked ? '\0' : '#';
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            captchaLabel.Text = CaptchaGenerator.Generate(out captchaValue);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            accessManager.Close();
            Application.Exit();
        }
    }
}
