using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDiskSpaceRepository
    {
        public DirectoryInfo[] GetDirectories(string path);
        public FileInfo[] GetFiles(string path);
    }
}
