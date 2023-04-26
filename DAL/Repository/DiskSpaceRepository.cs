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
        public List<DirectoryInfoDTO> GetDirectories(string path)
        {
            var dirs = new List<DirectoryInfoDTO>();
            
            if (Directory.Exists(path))
            {
                var dirsIO = new DirectoryInfo(path).GetDirectories();

                dirs = dirsIO.Select(x => new DirectoryInfoDTO() { Path = x.FullName, 
                    DirectoryName = x.Name }).ToList();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting directories.");
            }
            return dirs;
        }
        public List<FileInfoDTO> GetFiles(string path)
        {
            var files = new List<FileInfoDTO>();
            if (Directory.Exists(path))
            {
                var filesIO = new DirectoryInfo(path).GetFiles();
                files = filesIO.Select(x => new FileInfoDTO() { Name = x.Name, Size = x.Length.ToString(),
                    BytesSize = x.Length, Extension = x.Extension }).ToList();
            }
            else
            {
                throw new FileNotFoundException($"This path {path} does not exist for getting files.");
            }
            return files;
        }
    }
}
