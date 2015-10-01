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
    public partial class UsersActivityForm : Form
    {
        private MainForm mainForm;

        public UsersActivityForm(MainForm main, AccessManager accessManager)
        {
            InitializeComponent();
            mainForm = main;
            accessManager.ActivityLog.Reverse();
            foreach (var str in accessManager.ActivityLog)
            {
                textBox1.AppendText(str + "\n");
            }
            accessManager.ActivityLog.Reverse();
        }

        private void UsersActivityForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainForm.Enabled = true;
        }

    }
}
