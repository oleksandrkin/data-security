using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public partial class AddUserForm : Form
    {
        private MainForm mainForm;

        private AccessManager accessManager;
            
        public AddUserForm(MainForm main, AccessManager accManager)
        {
            InitializeComponent();
            mainForm = main;
            accessManager = accManager;
            InitiaizeAccessPanel();
        }

        private List<Tuple<string, CheckBox, CheckBox>> chechBoxs = new List<Tuple<string, CheckBox, CheckBox>>(); 

        private void InitiaizeAccessPanel()
        {
            DirectoryInfo[] directories = accessManager.DirectoryInfo.GetDirectories();
            int dx = 10;
            int dy = 10;
            foreach (var directory in directories)
            {
                Label nameLabel = new Label
                {
                    Location = new Point(dx, dy),
                    Text = directory.Name, 
                    AutoSize = true
                };
                panel1.Controls.Add(nameLabel);

                CheckBox readAccessCheckBox = new CheckBox
                {
                    Location = new Point(nameLabel.Right + dx, dy - 5),
                    Text = @"Read access",
                    Checked = false
                };
                panel1.Controls.Add(readAccessCheckBox);

                CheckBox writeAccessCheckBox = new CheckBox
                {
                    Location = new Point(readAccessCheckBox.Right + dx, dy - 5),
                    Text = @"Write accees",
                    Checked = false
                };
                panel1.Controls.Add(writeAccessCheckBox);

                chechBoxs.Add(new Tuple<string, CheckBox, CheckBox>(nameLabel.Text, readAccessCheckBox, writeAccessCheckBox));

                dx = 10;
                dy = 10 + nameLabel.Bottom;
            }
            panel1.AutoScroll = true;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            List<Directory> directories = new List<Directory>();
            foreach (var tuple in chechBoxs)
            {
                if (tuple.Item2.Checked || tuple.Item2.Checked)
                    directories.Add(new Directory(tuple.Item1, tuple.Item2.Checked, tuple.Item3.Checked));
            }
            User newUser = new User(loginTextBox.Text,
                passTextBox.Text,
                (isAdminCheckBox.Checked ? AccessType.Admin : AccessType.User),
                directories);
            for(int i = 0; i < accessManager.Users.Count; i++)
            {
                if (accessManager.Users[i].Name == newUser.Name)
                {
                    accessManager.Users[i] = newUser;
                    mainForm.Enabled = true;
                    this.Close();
                }
            }
            accessManager.Users.Add(newUser);
            mainForm.Enabled = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            mainForm.Enabled = true;
            this.Close();
        }
    }
}
