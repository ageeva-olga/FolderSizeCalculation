using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileInfo = Logic.Models.FileInfo;

namespace Logic.Repository
{
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        private IDiskSpaceProcessor _diskSpaceProc;
        public DiskSpaceRepository(IDiskSpaceProcessor diskSpaceProc)
        {
            _diskSpaceProc = diskSpaceProc;
        }
        public List<FileInfo> GetFiles(string path)
        {
            var filesInfo = new List<FileInfo>();

            if (Directory.Exists(path))
            {
                var dirs = new DirectoryInfo(path).GetDirectories();
                var files = new DirectoryInfo(path).GetFiles();

                foreach (var dir in dirs)
                {
                    filesInfo.Add( new FileInfo() { Name = dir.Name, Size = _diskSpaceProc.SumSizeDirectories(new DirectoryInfo[] { dir }).ToString(),
                    IsDirectory = true});
                }

                foreach (var file in files)
                {
                    filesInfo.Add(new FileInfo() { Name = file.Name, Size = new System.IO.FileInfo(file.FullName).Length.ToString(), 
                    Extension = file.Extension, IsDirectory = false});
                }
            }
            var sortedFilesInfo = filesInfo.OrderBy(x => long.Parse(x.Size)).ToList();

            foreach(var sortFileInfo in sortedFilesInfo)
            {
                sortFileInfo.Size = _diskSpaceProc.TranformFromBytes(long.Parse(sortFileInfo.Size));
            }

            return sortedFilesInfo;
        }

        private long SumSizeDirectories(DirectoryInfo[] dirs)
        {
            long sum = 0;

            foreach (var dir in dirs)
            {
                var dirsInDir = new DirectoryInfo(dir.FullName).GetDirectories();
                var filesInDir = new DirectoryInfo(dir.FullName).GetFiles();

                if(dirsInDir.Length != 0)
                {
                    foreach (var file in filesInDir)
                    {
                        sum += new System.IO.FileInfo(file.FullName).Length;
                    }

                    sum += SumSizeDirectories(dirsInDir); 
                }

                else
                {
                    foreach (var file in filesInDir)
                    {
                        sum += new System.IO.FileInfo(file.FullName).Length;
                    }
                }
            }

            return sum;
        }
    }
}
