using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class FileInfoDTO
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public long BytesSize { get; set; }
        public string Extension { get; set; }
    }
}
