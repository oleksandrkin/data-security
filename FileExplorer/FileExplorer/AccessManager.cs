using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileExplorer
{
    public class AccessManager
    {
        public List<User> Users { get; set; }

        public AccessManager()
        {  
            ReadAccessTable();
            ReadActivityLog();
            CriticalAccess = 0;
            CriticalCaptcha = 0;
            CriticalDelete = 0;

        }

        public User CurrentUser { get; set; }

        public bool Authorization(string name, string password)
        {
            foreach (User user in Users.Where(user => user.Name == name && user.Password == password))
            {
                if (CurrentUser != user)
                {
                    CriticalDelete = 0;
                    CurrentUser = user;
                }
                AddToActivityLog(CurrentUser, "log in successfully");
                return true;
            }
            AddToActivityLog(String.Format("Fail to log in with login: \"{0}\" and pass: \"{1}\"", name, password));
            return false;
        }

        private void ReadAccessTable()
        {           
            BinaryFormatter formatter = new BinaryFormatter();
            using(Stream accStream = new FileStream("..//..//Resources//AccessTable.act", FileMode.Open))
            {
                if(accStream.Length != 0)
                    Users = (List<User>) formatter.Deserialize(accStream);
                else
                {
                     Users = new List<User> {new User("admin", "admin", AccessType.Admin)};
                }
            }
        }

        public void WriteAccessTable()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream accStream = new FileStream("..//..//Resources//AccessTable.act",FileMode.OpenOrCreate))
            {
                formatter.Serialize(accStream, Users);
            }
        }

        public DirectoryInfo DirectoryInfo  = new DirectoryInfo(@"../../Computer");
       
        private TreeView treeView;

        private bool CheckDirectoryAccess(DirectoryInfo directory)
        {
            if (CurrentUser.AccessType == AccessType.Admin) return true;
            else return CurrentUser.Directories.Any(d => d.Name == directory.Name);
        }

        public void Show(TreeView tView)
        {
            treeView = tView;
            try
            {
                DirectoryInfo[] directories = DirectoryInfo.GetDirectories();

                if (directories.Length > 0)
                {
                    foreach (DirectoryInfo directory in directories)
                    {
                        if (CheckDirectoryAccess(directory))
                        {
                            TreeNode node = treeView.Nodes[0].Nodes.Add(directory.Name);
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                if (file.Exists)
                                {
                                    treeView.Nodes[0].Nodes[node.Index].Nodes.Add(file.Name);
                                }
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

        public void Copy()
        {
            if (treeView.SelectedNode != null)
            {
                try
                {
                    DirectoryInfo[] directories = DirectoryInfo.GetDirectories();

                    if (directories.Length > 0)
                    {
                        foreach (DirectoryInfo directory in directories)
                        {
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                if (file.Exists && file.Name == treeView.SelectedNode.Text)
                                {
                                    StringCollection filePath = new StringCollection();
                                    filePath.Add(file.FullName);
                                    Clipboard.SetFileDropList(filePath);
                                    AddToActivityLog(CurrentUser, String.Format("copy \"{0}\" in folder \"{1}\"", file.Name, directory.Name));
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

        public void Paste()
        {
            if (treeView.SelectedNode != null)
            {
                bool copy = false;
                try
                {
                    DirectoryInfo[] directories = DirectoryInfo.GetDirectories();
                    if (directories.Length > 0)
                    {
                        foreach (DirectoryInfo directory in directories)
                        {
                            if (directory.Name == treeView.SelectedNode.Text && Clipboard.ContainsFileDropList())
                            {
                                foreach (string file in Clipboard.GetFileDropList())
                                {
                                    string targetDir = DirectoryInfo.FullName + @"\" + directory.Name;
                                    File.Copy(Path.Combine(file.Replace(Path.GetFileName(file), ""), Path.GetFileName(file)), Path.Combine(targetDir, Path.GetFileName(file)), true);
                                    AddToActivityLog(CurrentUser, String.Format("paste \"{0}\" in folder \"{1}\"", Path.GetFileName(file), directory.Name));
                                }
                                copy = true;
                            }
                        }
                    }

                    if (copy)
                    {
                        foreach (string file in Clipboard.GetFileDropList())
                        {
                            TreeNode node = treeView.Nodes[0].Nodes[treeView.SelectedNode.Index].Nodes.Add(Path.GetFileName(file));
                        }
                        copy = false;
                    }
                }
                catch (Exception ex)
                {
                    copy = false;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        // TODO: fixed deleting files in different folders
        public void Delete()
        {
            if (treeView.SelectedNode != null)
            {
                bool deleted = false;
                try
                {
                    DirectoryInfo[] directories = DirectoryInfo.GetDirectories();

                    if (directories.Length > 0)
                    {
                        foreach (DirectoryInfo directory in directories)
                        {
                            foreach (FileInfo file in directory.GetFiles())
                            {
                                if (file.Exists && file.Name == treeView.SelectedNode.Text)
                                {
                                    file.Delete();
                                    deleted = true;
                                    bool control = ++CriticalDelete > CriticalDeleteControl;
                                    AddToActivityLog(CurrentUser, String.Format("delete \"{0}\" from folder \"{1}\"; Control number: {3}", file.Name, directory.Name, CriticalDelete), control);
                                }
                            }

                            if (treeView.SelectedNode.Text == directory.Name)
                            {
                                foreach (FileInfo file in directory.GetFiles())
                                {
                                    if (file.Exists)
                                        file.Delete();
                                }
                                directory.Delete();
                                AddToActivityLog(CurrentUser, String.Format("delete  folder \"{0}\"", directory.Name));
                                deleted = true;
                            }
                        }
                    }

                    if (deleted)
                        treeView.SelectedNode.Remove();
                }
                catch (Exception ex)
                {
                    deleted = false;
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public List<String> ActivityLog { get; set; }

        public List<String> CriticalActivityLog { get; set; }

        public int CriticalAccess { get; set; }
        public int CriticalAccessControl = 2;

        public int CriticalDelete { get; set; }
        public int CriticalDeleteControl = 2;

        public int CriticalCaptcha { get; set; }
        public int CriticalCaptchaControl = 2;

        public void ReadActivityLog()
        {
            ActivityLog = File.ReadAllLines(@"../../Resources/ActivityLog.txt").ToList();
            CriticalActivityLog = File.ReadAllLines(@"../../Resources/CriticalActivityLog.txt").ToList();
        }

        public void WriteActivityLog()
        {
            File.WriteAllLines(@"../../Resources/ActivityLog.txt", ActivityLog);
            File.WriteAllLines(@"../../Resources/CriticalActivityLog.txt", CriticalActivityLog);
        }

        public void AddToActivityLog(String message, bool critical = false)
        {
            ActivityLog.Add(String.Format("{0} : {1}", DateTime.Now, message));
            if (critical) CriticalActivityLog.Add(String.Format("{0} : {1}", DateTime.Now, message));
        }

        public void AddToActivityLog(User user, String message, bool critical = false)
        {
            ActivityLog.Add(String.Format("{0} : User: {1} {2}", DateTime.Now, user.Name, message));
            if (critical) CriticalActivityLog.Add(String.Format("{0} : User: {1} {2}", DateTime.Now, user.Name, message));
        }

        public void Close()
        {
            WriteAccessTable();
            WriteActivityLog();
        }
    }
}
