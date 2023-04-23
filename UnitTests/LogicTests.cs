using Logic.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests
{
    public class LogicTests
    {
        public string path = "D:\\Английский репетитор";
        [Test]
        public void Test1()
        {
            var loggerMock = new Mock<ILogger>();
            var diskSpaceRepo = new Mock<IDiskSpaceRepository>();
            Assert.Pass();
        }
    }
}
