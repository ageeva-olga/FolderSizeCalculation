using DAL.DTO;
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
    public class LogicTests
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
            file1 = new FileInfoDTO() { Name = "file1", Size = "3001200", Extension = ".pdf" };
            file2 = new FileInfoDTO() { Name = "file2", Size = "1000", Extension = ".pdf" };
            file3 = new FileInfoDTO() { Name = "file3", Size = "100", Extension = ".exe" };
            file4 = new FileInfoDTO() { Name = "file4", Size = "1000000002", Extension = ".pdf" };
        }

        private List<DirectoryInfoModel> CompletionDiskSpaceProcessor()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new DirectoryInfoDTO[] { dir1 });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new FileInfoDTO[] { file1, file2, file3, file4 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path);
            return getDirInfo;
        }

        private List<DirectoryInfoModel> CompletionDiskSpaceProcessorForDir()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new DirectoryInfoDTO[] { dir1, dir3 });
            diskSpaceRepo.Setup(x => x.GetDirectories(dir1.Path)).Returns(new DirectoryInfoDTO[] { dir2 });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new FileInfoDTO[] { file1, file4, file3 });
            diskSpaceRepo.Setup(x => x.GetFiles(dir1.Path)).Returns(new FileInfoDTO[] { file2, file3 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path);
            return getDirInfo;
        }

        [Test]
        public void FileSortingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessor();

            Assert.AreEqual(dir1.DirectoryName, getDirInfo[0].Name);
            Assert.AreEqual(file3.Name, getDirInfo[1].Name);
            Assert.AreEqual(file2.Name, getDirInfo[2].Name);
            Assert.AreEqual(file1.Name, getDirInfo[3].Name);
            Assert.AreEqual(file4.Name, getDirInfo[4].Name);
        }
        
        [Test]
        public void DimensionTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessor();

            Assert.AreEqual("0Byte", getDirInfo[0].Size);
            Assert.AreEqual("100Byte", getDirInfo[1].Size);
            Assert.AreEqual("1Kb", getDirInfo[2].Size);
            Assert.AreEqual("3.0012Mb", getDirInfo[3].Size);
            Assert.AreEqual("1.000000002Gb", getDirInfo[4].Size);
        }

        [Test]
        public void DirectoriesSortingTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorForDir();

            Assert.AreEqual(dir3.DirectoryName, getDirInfo[0].Name);
            Assert.AreEqual(file3.Name, getDirInfo[1].Name);
            Assert.AreEqual(dir1.DirectoryName, getDirInfo[2].Name);
            Assert.AreEqual(file1.Name, getDirInfo[3].Name);
            Assert.AreEqual(file4.Name, getDirInfo[4].Name);
        }

        [Test]
        public void DirectoriesSizeTest()
        {
            var getDirInfo = CompletionDiskSpaceProcessorForDir();

            Assert.AreEqual(dir1.DirectoryName, getDirInfo[2].Name);
            Assert.AreEqual("1.1Kb", getDirInfo[2].Size);
        }

        [Test]
        public void ValidationTrueTest()
        {
            var validation = new Validation();
            var resValidation = validation.Validate(path);

            Assert.AreEqual(true, resValidation);
        }

        [Test]
        public void ValidationErrorTest()
        {
            var validation = new Validation();
            var resValidation = validation.Validate("");

            Assert.AreEqual(false, resValidation);
        }

        [Test]
        public void ValidationGetErrorsTest()
        {
            var validation = new Validation();
            var resValidation = validation.Validate("");
            var resGetErrors = validation.GetErrors();

            Assert.AreEqual("This path  is null.", resGetErrors[0]);
        }
    }
}
