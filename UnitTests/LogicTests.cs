using DAL.DTO;
using DAL.Interfaces;
using Logic.Interfaces;
using Logic.Processor;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    public class LogicTests
    {
        public DirectoryInfoDTO dir;
        public FileInfoDTO file1;
        public FileInfoDTO file2;
        public string path;

        [OneTimeSetUp]
        public void NewDirAndFiles()
        {
            path = "D:\\";
            dir = new DirectoryInfoDTO() { Path = "D:\\Folder", DirectoryName = "Folder" };
            file1 = new FileInfoDTO() { Name = "file1", Size = "301200", Extension = ".pdf" };
            file2 = new FileInfoDTO() { Name = "file2", Size = "1000", Extension = ".pdf" };
        }

        [Test]
        public void Test1()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            diskSpaceRepo.Setup(x => x.GetDirectories(path)).Returns(new DirectoryInfoDTO[] { dir });
            diskSpaceRepo.Setup(x => x.GetFiles(path)).Returns(new FileInfoDTO[] { file1 , file2 });
            var validator = new Mock<IValidation>();
            validator.Setup(x => x.Validate(path)).Returns(true);

            var diskSpaceProc = new DiskSpaceProcessor(diskSpaceRepo.Object, loggerMock.Object, validator.Object);
            var getDirInfo = diskSpaceProc.GetDirectoryInfo(path);

            Assert.AreEqual(dir.DirectoryName, getDirInfo[0].Name);

            Assert.AreEqual(file2.Name, getDirInfo[1].Name);
            Assert.AreEqual("1Kb", getDirInfo[1].Size);

            Assert.AreEqual(file1.Name, getDirInfo[2].Name);
            Assert.AreEqual("301.2Kb", getDirInfo[2].Size);
        }
    }
}
