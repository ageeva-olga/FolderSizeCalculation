using Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectoryInfoModel = Logic.Models.DirectoryInfoModel;
using Microsoft.Extensions.Logging;
using System.Net;
using Logic.Models;
using DAL.Interfaces;
using DAL.DTO;

namespace Logic.Processor
{
    public class DiskSpaceProcessor : IDiskSpaceProcessor
    {
        private IValidation _validator;
        private IDiskSpaceRepository _diskSpaceRepo;
        private ILogger _logger;

        public DiskSpaceProcessor(IDiskSpaceRepository diskSpaceRepo, ILogger logger, IValidation validator)
        {
            _validator = validator;
            _diskSpaceRepo = diskSpaceRepo;
            _logger = logger;
        }

        public List<DirectoryInfoModel> GetDirectoryInfo(string path)
        {
            if (!_validator.Validate(path))
            {
                var error = _validator.GetErrors().First();
                _logger.LogError(error);

                throw new ArgumentNullException(error);

            }

            var filesInfo = new List<DirectoryInfoModel>();

            DirectoryInfoDTO[] dirs = null;
            try
            {
                dirs = _diskSpaceRepo.GetDirectories(path);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            FileInfoDTO[] files = null;
            try
            {
                files = _diskSpaceRepo.GetFiles(path);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            AddDirectories(filesInfo, dirs);

            AddFiles(filesInfo, files);
            var sortedFilesInfo = SortFileInfo(filesInfo);

            return sortedFilesInfo;
        }

        private List<DirectoryInfoModel> SortFileInfo(List<DirectoryInfoModel> filesInfo)
        {
            var sortedFilesInfo = filesInfo.OrderBy(x => long.Parse(x.Size)).ToList();

            foreach (var sortFileInfo in sortedFilesInfo)
            {
                sortFileInfo.Size = TranformFromBytes(long.Parse(sortFileInfo.Size));
            }

            return sortedFilesInfo;
        }

        private static void AddFiles(List<DirectoryInfoModel> filesInfo, FileInfoDTO[] files)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    filesInfo.Add(new DirectoryInfoModel()
                    {
                        Name = file.Name,
                        Size = file.Size,
                        Extension = file.Extension,
                        IsDirectory = false
                    });
                }
            }
        }

        private void AddDirectories(List<DirectoryInfoModel> filesInfo, DirectoryInfoDTO[] dirs)
        {
            if (dirs != null)
            {
                foreach (var dir in dirs)
                {
                    filesInfo.Add(new DirectoryInfoModel()
                    {
                        Name = dir.DirectoryName,
                        Size = SumSizeDirectories(new DirectoryInfoDTO[] { dir }).ToString(),
                        IsDirectory = true
                    });
                }
            }
        }

        private long SumSizeDirectories(DirectoryInfoDTO[] dirs)
        {
            long sum = 0;

            foreach (var dir in dirs)
            {
                DirectoryInfoDTO[] dirsInDirDTO = null;
                try
                {
                    dirsInDirDTO = _diskSpaceRepo.GetDirectories(dir.Path);
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }

                FileInfoDTO[] filesInDirDTO = null;
                try
                {
                    filesInDirDTO = _diskSpaceRepo.GetFiles(dir.Path);
                }
                catch (FileNotFoundException ex)
                {
                    _logger.LogError(ex, ex.Message);
                    throw;
                }

                if (dirsInDirDTO.Length != 0)
                {
                    foreach (var file in filesInDirDTO)
                    {
                        sum += long.Parse(file.Size);
                    }

                    sum += SumSizeDirectories(dirsInDirDTO);
                }

                else
                {
                    foreach (var file in filesInDirDTO)
                    {
                        sum += long.Parse(file.Size);
                    }
                }
            }

            return sum;
        }

        private string TranformFromBytes(long size)
        {
            var nameDimension = new string[] { "Byte" , "Kb" , "Mb" , "Gb" };
            var count = 0;
            var doubleSize = (double)size;

            while (doubleSize >= 1000 && count < 3)
            {
                doubleSize = doubleSize / 1000;
                count++;
            }
            return doubleSize.ToString() + nameDimension[count];
        }
    }
}
