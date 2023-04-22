using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Logic.Repository
{
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        public DirectoryInfo[] GetDirectories(string path)
        {
            DirectoryInfo[] dirs = null;
            
            if (Directory.Exists(path))
            {
                dirs = new DirectoryInfo(path).GetDirectories();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting directories.");
            }
            return dirs;
        }
        public FileInfo[] GetFiles(string path)
        {
            FileInfo[] files = null;
            if (Directory.Exists(path))
            {
                files = new DirectoryInfo(path).GetFiles();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting files.");
            }
            return files;
        }
    }
}
