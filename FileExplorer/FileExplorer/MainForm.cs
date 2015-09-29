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
            (new LoginForm(accessManager, this)).Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            accessManager.Close();
            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            accessManager.Close();
            Application.Exit();
        }

        private void showFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            (new LoginForm(accessManager, this)).Show();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            adminToolStripMenuItem.Visible = accessManager.CurrentUser.AccessType == AccessType.Admin;
            accessManager.Show(treeView1);
        }

        private void addUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            (new AddUserForm(this, accessManager)).Show();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            accessManager.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            accessManager.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            accessManager.Delete();
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            (new UsersActivityForm(this, accessManager)).Show();
        }
    }
}
