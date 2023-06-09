﻿using DAL.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IDiskSpaceRepository
    {
        public List<DirectoryInfoDTO> GetDirectories(string path);
        public List<FileInfoDTO> GetFiles(string path);
    }
}
