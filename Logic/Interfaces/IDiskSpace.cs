using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDiskSpace
    {
        public Dictionary<string, long> GetFiles(string path);
    }
}
