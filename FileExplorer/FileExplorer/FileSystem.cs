using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public class FileSystem
    {
        private DirectoryInfo directoryInfo;
        private TreeView treeView;

        public FileSystem(TreeView tv)
        {
            directoryInfo = new DirectoryInfo(@"../../Computer");
            treeView = tv;
        }

        public void Show()
        {
            try
            {
                DirectoryInfo[] directories = directoryInfo.GetDirectories();

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    if (file.Exists)
                    {
                        TreeNode nodes = treeView.Nodes[0].Nodes.Add(file.Name);
                    }
                }

                if (directories.Length > 0)
                {
                    foreach (DirectoryInfo directory in directories)
                    {
                        TreeNode node = treeView.Nodes[0].Nodes.Add(directory.Name);
                        foreach (FileInfo file in directory.GetFiles())
                        {
                            if (file.Exists)
                            {
                                TreeNode nodes = treeView.Nodes[0].Nodes[node.Index].Nodes.Add(file.Name);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
