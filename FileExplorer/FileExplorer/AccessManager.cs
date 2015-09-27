using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public class AccessManager
    {
        private List<User> users;
 
        public AccessManager()
        {  
            ReadAccessTable();
        }

        public bool Authorization(string name, string password)
        {
            return users.Any(user => user.Name == name && user.Password == password);
        }

        private void ReadAccessTable()
        {           
            BinaryFormatter formatter = new BinaryFormatter();
            using(Stream accStream = new FileStream("..//..//Resources//AccessTable.act", FileMode.Open))
            {
                users = (List<User>) formatter.Deserialize(accStream);
            }
        }

        private void WriteAccessTable()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (Stream accStream = new FileStream("..//..//Resources//AccessTable.act",FileMode.OpenOrCreate))
            {
                formatter.Serialize(accStream, users);
            }
        }

        private void AddUser(User admin, User newUser)
        {
            
        }
    }
}
