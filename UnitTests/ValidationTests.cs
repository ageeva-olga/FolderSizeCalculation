using Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    class ValidationTests
    {
        public string path;

        [OneTimeSetUp]
        public void NewDirAndFiles()
        {
            path = "D:\\";
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
