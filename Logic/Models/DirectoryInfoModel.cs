using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class DirectoryInfoModel
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public string? Extension { get; set; }
        public bool IsDirectory { get; set; }
    }
}
