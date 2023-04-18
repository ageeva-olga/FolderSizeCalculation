using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository
{
    public class DiskSpaceRepository : IDiskSpace
    {
        public Dictionary<string, long> GetFiles(string path)
        {
            var filesInfo = new Dictionary<string, long>();

            if (Directory.Exists(path))
            {
                var dirs = new DirectoryInfo(path).GetDirectories();
                var files = new DirectoryInfo(path).GetFiles();

                foreach (var dir in dirs)
                {
                    filesInfo.Add(dir.Name, SumSizeDirectories(new DirectoryInfo[] { dir }));
                }

                foreach (var file in files)
                {
                    filesInfo.Add(file.Name, new FileInfo(file.FullName).Length);
                }
            }

            var sortedDict = filesInfo.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return sortedDict;
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
                        sum += new FileInfo(file.FullName).Length;
                    }

                    sum += SumSizeDirectories(dirsInDir); 
                }

                else
                {
                    foreach (var file in filesInDir)
                    {
                        sum += new FileInfo(file.FullName).Length;
                    }
                }
            }

            return sum;
        }
    }
}
