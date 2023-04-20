﻿using Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Interfaces
{
    public interface IDiskSpaceRepository
    {
        public List<FileInfo> GetFiles(string path);
    }
}