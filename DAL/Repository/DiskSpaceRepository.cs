using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Logic.Repository
{
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        private ILogger _logger;
        public DiskSpaceRepository(ILogger logger)
        {
            _logger = logger;
        }

        public DirectoryInfo[] GetDirectories(string path)
        {
            DirectoryInfo[] dirs = null;
            if(path == null)
            {
                var infoEx = String.Format("This path {0} is null.", path);
                _logger.LogInformation(infoEx);
            }
            if (Directory.Exists(path))
            {
                dirs = new DirectoryInfo(path).GetDirectories();
            }
            else
            {
                var infoEx = String.Format("This path {0} does not exist.", path);
                _logger.LogInformation(infoEx);
            }
            return dirs;
        }
        public FileInfo[] GetFiles(string path)
        {
            FileInfo[] files = null;
            if (path == null)
            {
                var infoEx = String.Format("This path {0} is null.", path);
                _logger.LogInformation(infoEx);
            }
            if (Directory.Exists(path))
            {
                files = new DirectoryInfo(path).GetFiles();
            }
            else
            {
                var infoEx = String.Format("This path {0} does not exist.", path);
                _logger.LogInformation(infoEx);
            }
            return files;
        }
    }
}
