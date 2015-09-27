using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public enum AccessType
    {
        Admin,
        User
    }

    [Serializable]
    public class User
    {
        public string Name { get; private set; }

        public string Password { get; set; }

        public AccessType AccessType { get; set; }

        public User(string name, string password, AccessType accessType)
        {
            Name = name;
            Password = password;
            AccessType = accessType;
        }
    }
}
