using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class DirectoryInfoModel
    {
        public List<DirectoryModel> DirectoryModel{ get; set; }
        public List<FileModel> FileModel { get; set; }
        
    }
}
