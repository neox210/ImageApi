using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Services;
using ImageApi_Tests.TestFiles;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ImageApi_Tests
{
    [TestFixture]
    public class FileSystemServiceTest
    {
        private readonly IFileSystemService _fileSystemService;
        public FileSystemServiceTest()
        {
            var settings = TestHelper.GetImageServiceSettings();
            _fileSystemService = new FileSystemService(settings);
        }

        [OneTimeTearDown]
        public void ClearTestData()
        {
            DirectoryInfo dir = new DirectoryInfo(TestHelper.GetImageServiceSettings().FileSystemServiceSettings.ImageDirectory);
            dir.Delete(true);
        }  

        [Test]
        public async Task SaveImage_Returns_True()
        {
            var img = TestHelper.GetSingleImageAsync().Result;
            var name = Guid.NewGuid() + ".jpeg";

            var result = await _fileSystemService.SaveImageAsync(name,img);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task Loadimage_Returns_ArrayOfBytes()
        {
            var img = TestHelper.GetSingleImageAsync().Result;
            var name = Guid.NewGuid() + ".jpeg";

            var savedImg = await _fileSystemService.SaveImageAsync(name, img);

            var result = await _fileSystemService.LoadImageAsync(name);
            Assert.IsInstanceOf(typeof(byte[]), result);
        }

        ////////////////////
        //Exceptions tests//
        ////////////////////

        [Test]
        public void Loadimage_When_Image_Not_Fount_Throws_FileSystemServiceException()
        {
            var img = TestHelper.GetSingleImageAsync().Result;
            var name = Guid.NewGuid() + ".jpeg";

            var ex =Assert.ThrowsAsync<FileSystemServiceException>(async () => await _fileSystemService.LoadImageAsync(name));
            
            Assert.IsInstanceOf(typeof(FileNotFoundException), ex.InnerException);
        }
    }
}
