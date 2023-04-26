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

        public DirectoryInfoModel GetDirectoryInfo(string path, bool isAscending)
        {
            if (!_validator.Validate(path))
            {
                var error = _validator.GetErrors().First();
                _logger.LogError(error);

                throw new ArgumentNullException(error);

            }

            List<DirectoryInfoDTO> dirs = GetDirectories(path);
            List<FileInfoDTO> files = GetFiles(path);

            var directoryModels = new List<DirectoryModel>();
            directoryModels = ProcessDirectories(dirs);

            var fileModels = new List<FileModel>();
            fileModels = ProcessFiles(files);

            if (isAscending)
            {
                directoryModels = directoryModels.OrderBy(x => x.BytesSize).ToList();
                fileModels = fileModels.OrderBy(x => x.BytesSize).ToList();
            }
            else
            {
                directoryModels = directoryModels.OrderByDescending(x => x.BytesSize).ToList();
                fileModels = fileModels.OrderByDescending(x => x.BytesSize).ToList();
            }

            foreach (var directoryModel in directoryModels)
            {
                directoryModel.Size = TranformFromBytes(directoryModel.BytesSize);
            }

            foreach (var fileModel in fileModels)
            {
                fileModel.Size = TranformFromBytes(fileModel.BytesSize);
            }

            var directoryInfoModel = new DirectoryInfoModel() { DirectoryModel = directoryModels, 
                FileModel = fileModels};

            return directoryInfoModel;
        }

        private static List<FileModel> ProcessFiles(List<FileInfoDTO> files)
        {
            var filesInfo = new List<FileModel>() { };

            if (files != null)
            {
                foreach (var file in files)
                {
                    filesInfo.Add(new FileModel()
                    {
                        Name = file.Name,
                        Size = file.BytesSize.ToString(),
                        BytesSize = file.BytesSize,
                        Extension = file.Extension
                    });
                }
            }

            return filesInfo;
        }

        private List<DirectoryModel> ProcessDirectories(List<DirectoryInfoDTO> dirs)
        {
            var directoriesInfo = new List<DirectoryModel>() { };

            if (dirs != null)
            {
                foreach (var dir in dirs)
                {
                    var size = SumSizeDirectories(new List<DirectoryInfoDTO> { dir });
                    directoriesInfo.Add(new DirectoryModel()
                    {
                        Name = dir.DirectoryName,
                        Size = size.ToString(),
                        BytesSize = size
                    });
                }
            }

            return directoriesInfo;
        }

        private long SumSizeDirectories(List<DirectoryInfoDTO> dirs)
        {
            long sum = 0;

            foreach (var dir in dirs)
            {
                List<DirectoryInfoDTO> dirsInDirDTO = GetDirectories(dir.Path);
                List<FileInfoDTO> filesInDirDTO = GetFiles(dir.Path);

                if (dirsInDirDTO != null)
                {
                    sum += SumSizeDirectories(dirsInDirDTO);
                }

                if (dirsInDirDTO != null)
                {
                    foreach (var file in filesInDirDTO)
                    {
                        sum += file.BytesSize;
                    }
                }
            }

            return sum;
        }

        private List<DirectoryInfoDTO> GetDirectories(string path)
        {
            var dirsInDirDTO = new List<DirectoryInfoDTO>() { };
            try
            {
                dirsInDirDTO = _diskSpaceRepo.GetDirectories(path);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return dirsInDirDTO;
        }

        private List<FileInfoDTO> GetFiles(string path)
        {
            var filesInDirDTO = new List<FileInfoDTO>() { };
            try
            {
                filesInDirDTO = _diskSpaceRepo.GetFiles(path);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

            return filesInDirDTO;
        }

        private string TranformFromBytes(long size)
        {
            var nameDimension = new string[] { "Byte", "Kb", "Mb", "Gb" };
            var count = 0;
            var doubleSize = (double)size;

            while (doubleSize >= 1000 && count < 3)
            {
                doubleSize = doubleSize / 1000;
                count++;
            }

            return Math.Round(doubleSize,2).ToString() + nameDimension[count];
        }
    }
}
