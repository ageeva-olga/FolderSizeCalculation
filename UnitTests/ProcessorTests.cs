﻿using DAL.DTO;
using DAL.Interfaces;
using Logic;
using Logic.Interfaces;
using Logic.Models;
using Logic.Processor;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
    public class ProcessorTests
    {
        public DirectoryInfoDTO dir1;
        public DirectoryInfoDTO dir2;
        public DirectoryInfoDTO dir3;
        public FileInfoDTO file1;
        public FileInfoDTO file2;
        public FileInfoDTO file3;
        public FileInfoDTO file4;
        public string path;

        [OneTimeSetUp]
        public void NewDirAndFiles()
        {
            path = "D:\\";
            dir1 = new DirectoryInfoDTO() { Path = "D:\\Folder", DirectoryName = "Folder" };
            dir2 = new DirectoryInfoDTO() { Path = "D:\\Folder2", DirectoryName = "Folder2" };
            dir3 = new DirectoryInfoDTO() { Path = "D:\\Folder3", DirectoryName = "Folder3" };
            file1 = new FileInfoDTO() { Name = "file1", Size = "3001200", BytesSize = 3001200, Extension = ".pdf" };
            file2 = new FileInfoDTO() { Name = "file2", Size = "1000", BytesSize = 1000, Extension = ".pdf" };
            file3 = new FileInfoDTO() { Name = "file3", Size = "100", BytesSize = 100, Extension = ".exe" };
            file4 = new FileInfoDTO() { Name = "file4", Size = "1000000002", BytesSize = 1000000002, Extension = ".pdf" };
        }

        private List<DirectoryInfoModel> CompletionDiskSpaceProcessorAscending()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new List<DirectoryInfoDTO>() { dir1 });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new List<FileInfoDTO>() { file1, file2, file3, file4 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path, true);
            return getDirInfo;
        }

        private List<DirectoryInfoModel> CompletionDiskSpaceProcessorDescending()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new List<DirectoryInfoDTO>() { dir1 });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new List<FileInfoDTO>() { file1, file2, file3, file4 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path, false);
            return getDirInfo;
        }

        private List<DirectoryInfoModel> CompletionDiskSpaceProcessorForDir()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new List<DirectoryInfoDTO>() { dir1, dir3 });
            diskSpaceRepo.Setup(x => x.GetDirectories(dir1.Path)).Returns(new List<DirectoryInfoDTO>() { dir2 });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new List<FileInfoDTO>() { file1, file4, file3 });
            diskSpaceRepo.Setup(x => x.GetFiles(dir1.Path)).Returns(new List<FileInfoDTO>() { file2, file3 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path, true);
            return getDirInfo;
        }

        [Test]
        public void FileSortingAscendingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorAscending();

            Assert.AreEqual(dir1.DirectoryName, getDirInfo[0].Name);
            Assert.AreEqual(file3.Name, getDirInfo[1].Name);
            Assert.AreEqual(file2.Name, getDirInfo[2].Name);
            Assert.AreEqual(file1.Name, getDirInfo[3].Name);
            Assert.AreEqual(file4.Name, getDirInfo[4].Name);
        }

        [Test]
        public void FileSortingDescendingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorDescending();

            Assert.AreEqual(dir1.DirectoryName, getDirInfo[0].Name);
            Assert.AreEqual(file3.Name, getDirInfo[4].Name);
            Assert.AreEqual(file2.Name, getDirInfo[3].Name);
            Assert.AreEqual(file1.Name, getDirInfo[2].Name);
            Assert.AreEqual(file4.Name, getDirInfo[1].Name);
        }

        [Test]
        public void DimensionAscendingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorAscending();

            Assert.AreEqual("0Byte", getDirInfo[0].Size);
            Assert.AreEqual("100Byte", getDirInfo[1].Size);
            Assert.AreEqual("1Kb", getDirInfo[2].Size);
            Assert.AreEqual("3Mb", getDirInfo[3].Size);
            Assert.AreEqual("1Gb", getDirInfo[4].Size);
        }

        [Test]
        public void DirectoriesAscendingSortingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorForDir();

            Assert.AreEqual(dir3.DirectoryName, getDirInfo[0].Name);
            Assert.AreEqual(dir1.DirectoryName, getDirInfo[1].Name);

            Assert.AreEqual(file3.Name, getDirInfo[2].Name);           
            Assert.AreEqual(file1.Name, getDirInfo[3].Name);
            Assert.AreEqual(file4.Name, getDirInfo[4].Name);
        }

        [Test]
        public void DirectoriesSizeTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorForDir();

            Assert.AreEqual(dir1.DirectoryName, getDirInfo[1].Name);
            Assert.AreEqual("1.1Kb", getDirInfo[1].Size);
        }


    }
}
