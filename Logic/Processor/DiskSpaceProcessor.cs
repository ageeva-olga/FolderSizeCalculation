using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Processor
{
    public class DiskSpaceProcessor : IDiskSpaceProcessor
    {
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
                transformSize = ((double)size / 1000000000).ToString() + "b";
            }
            return transformSize;
        }
    }
}
