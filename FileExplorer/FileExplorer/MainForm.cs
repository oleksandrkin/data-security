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
        private FileSystem fileSystem;

        public MainForm()
        {
            InitializeComponent();
            accessManager = new AccessManager();
            fileSystem = new FileSystem(treeView1);    
            (new LoginForm(accessManager, this)).Show();
            fileSystem.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            (new LoginForm(accessManager, this)).Show();
        }
    }
}
