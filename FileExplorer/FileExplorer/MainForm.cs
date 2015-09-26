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
    public partial class MainForm : Form
    {
        private AccessManager accessManager;
        
        public MainForm()
        {
            InitializeComponent();
            accessManager = new AccessManager();
            LoginForm loginForm = new LoginForm(accessManager, this);
            loginForm.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
