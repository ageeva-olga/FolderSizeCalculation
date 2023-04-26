using Logic.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDiskSpaceProcessor
    {
        public DirectoryInfoModel GetDirectoryInfo(string path, bool isAscending);
    }
}
