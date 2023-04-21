using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileInfo = Logic.Models.FileInfo;

namespace Logic.Processor
{
    public class DiskSpaceProcessor : IDiskSpaceProcessor
    {
        private IDiskSpaceRepository _diskSpaceRepo;

        public DiskSpaceProcessor(IDiskSpaceRepository diskSpaceRepo)
        {
            _diskSpaceRepo = diskSpaceRepo;
        }

        public List<FileInfo> GetFiles(string path)
        {
            var filesInfo = new List<FileInfo>();

            var dirs = _diskSpaceRepo.GetDirectories(path);
            var files = _diskSpaceRepo.GetFiles(path);

            if(dirs != null)
            {
                foreach (var dir in dirs)
                {
                    filesInfo.Add(new FileInfo()
                    {
                        Name = dir.Name,
                        Size = SumSizeDirectories(new DirectoryInfo[] { dir }).ToString(),
                        IsDirectory = true
                    });
                }
            }
            
            if(files != null)
            {
                foreach (var file in files)
                {
                    filesInfo.Add(new FileInfo()
                    {
                        Name = file.Name,
                        Size = new System.IO.FileInfo(file.FullName).Length.ToString(),
                        Extension = file.Extension,
                        IsDirectory = false
                    });
                }
            }
            
            var sortedFilesInfo = filesInfo.OrderBy(x => long.Parse(x.Size)).ToList();

            foreach (var sortFileInfo in sortedFilesInfo)
            {
                sortFileInfo.Size = TranformFromBytes(long.Parse(sortFileInfo.Size));
            }

            return sortedFilesInfo;
        }
         
        public long SumSizeDirectories(DirectoryInfo[] dirs)
        {
            long sum = 0;

            foreach (var dir in dirs)
            {
                var dirsInDir = new DirectoryInfo(dir.FullName).GetDirectories();
                var filesInDir = new DirectoryInfo(dir.FullName).GetFiles();

                if (dirsInDir.Length != 0)
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

        public string TranformFromBytes(long size)
        {
            string transformSize = "";
            if(size < 1000)
            {
                transformSize = size.ToString() + "Byte";
            }
            else if (1000 <= size && size < 1000000)
            {
                transformSize = ((double)size /1000).ToString() + "Kb";
            }
            else if (1000000 <= size && size < 1000000000)
            {
                transformSize = ((double)size / 1000000).ToString() + "Mb";
            }
            else
            {
                transformSize = ((double)size / 1000000000).ToString() + "Gb";
            }
            return transformSize;
        }
    }
}
