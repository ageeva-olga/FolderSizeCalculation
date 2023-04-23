using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using DAL.DTO;
using DAL.Interfaces;

namespace DAL.Repository
{
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        public DirectoryInfoDTO[] GetDirectories(string path)
        {
            DirectoryInfoDTO[] dirs = null;
            
            if (Directory.Exists(path))
            {
                var dirsIO = new DirectoryInfo(path).GetDirectories();

                dirs = dirsIO.Select(x => new DirectoryInfoDTO() { Path = x.FullName, DirectoryName = x.Name }).ToArray();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting directories.");
            }
            return dirs;
        }
        public FileInfoDTO[] GetFiles(string path)
        {
            FileInfoDTO[] files = null;
            if (Directory.Exists(path))
            {
                var filesIO = new DirectoryInfo(path).GetFiles();
                files = filesIO.Select(x => new FileInfoDTO() { Name = x.Name, Size = x.Length.ToString(), Extension = x.Extension }).ToArray();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting files.");
            }
            return files;
        }
    }
}
