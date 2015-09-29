using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    [Serializable]
    public class Directory
    {
        public string Name { get; set; }

        public bool ReadAccess { get; set; }

        public bool WriteAccess { get; set; }

        public Directory(string name, bool ra, bool wa)
        {
            Name = name;
            ReadAccess = ra;
            WriteAccess = wa;
        }
    }
}
