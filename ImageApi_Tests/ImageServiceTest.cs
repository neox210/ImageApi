using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Services;
using ImageApi_Tests.TestFiles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using NUnit.Framework;

namespace ImageApi_Tests
{
    [TestFixture]
    public class ImageServiceTest
    {
        private readonly IImageService _imageService;

        #region initial methods

        public ImageServiceTest()
        {
            List<IConvertedImage> thumbnails = new List<IConvertedImage>();

            var thumbnailMock = new Mock<IConvertedImage>();
            thumbnailMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            thumbnailMock.Setup(thumb => thumb.Format).Returns("jpeg");
            thumbnailMock.Setup(thumb => thumb.Prefix).Returns("min_");

            var thumbnailMock2 = new Mock<IConvertedImage>();
            thumbnailMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            thumbnailMock.Setup(thumb => thumb.Format).Returns("jpeg");
            thumbnailMock.Setup(thumb => thumb.Prefix).Returns("mid_");

            thumbnails.Add(thumbnailMock.Object);
            thumbnails.Add(thumbnailMock2.Object);

            var imgMock = new Mock<IConvertedImage>();
            imgMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            imgMock.Setup(thumb => thumb.Format).Returns("jpeg");
            imgMock.Setup(thumb => thumb.Prefix).Returns("orginal_");

            var fileSystemMock = new Mock<IFileSystemService>();
            fileSystemMock.Setup(fs => fs.SaveImageAsync(It.IsAny<string>(), It.IsAny<byte[]>())).ReturnsAsync(true);
            fileSystemMock.Setup(fs => fs.LoadImageAsync(It.IsAny<string>())).ReturnsAsync(TestHelper.GetSingleImageAsync().Result);

            var imageConverterMock = new Mock<IImageConverter>();
            imageConverterMock.Setup(ic => ic.ConvertImageToByteArrayAsync(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult(TestHelper.GetSingleImageAsync().Result));
            imageConverterMock.Setup(ic => ic.ConvertImageAsync(TestHelper.GetSingleImageAsync().Result))
                .ReturnsAsync(imgMock.Object);

            var thumbnailConverterMock = new Mock<IThumbnailConverter>();
            thumbnailConverterMock.Setup
                    (tc => tc.GetThumbnailsAsync(TestHelper.GetSingleImageAsync().Result))
                .ReturnsAsync(thumbnails);
            var loggerMock = new Mock<ILogger<IImageService>>();
           
            var dataMock = new Mock<IImageServiceData>();
            dataMock.Setup(fs => fs.FileSystemService.SaveImageAsync("name", new byte[1])).ReturnsAsync(true);
            dataMock.Setup(settings => settings.Settings).Returns(TestHelper.GetImageServiceSettings);
            dataMock.Setup(ic => ic.ImageConverter).Returns(imageConverterMock.Object);
            dataMock.Setup(tc => tc.ThumbnailConverter).Returns(thumbnailConverterMock.Object);
            dataMock.Setup(fs => fs.FileSystemService).Returns(fileSystemMock.Object);

            _imageService = new ImageService(dataMock.Object, loggerMock.Object);
        }

        private ImageService Init_ImageService_With_ImageConverter_Exception()
        {
            var loggerMock = new Mock<ILogger<IImageService>>();

            var imageConverterMock = new Mock<IImageConverter>();
            imageConverterMock.Setup(ic => ic.ConvertImageToByteArrayAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new ImageConverterException("Converter error", new Exception()));
            imageConverterMock.Setup(ic => ic.ConvertImageAsync(It.IsAny<byte[]>())).ThrowsAsync( new ImageConverterException("Converter error", new Exception()));

            var dataMock = new Mock<IImageServiceData>();
            dataMock.Setup(settings => settings.Settings).Returns(TestHelper.GetImageServiceSettings);
            dataMock.Setup(ic => ic.ImageConverter).Returns(imageConverterMock.Object);

            return new ImageService(dataMock.Object, loggerMock.Object);
        }

        private ImageService Init_ImageService_With_Thumbnail_Exception()
        {
            var imgMock = new Mock<IConvertedImage>();
            imgMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            imgMock.Setup(thumb => thumb.Format).Returns("jpeg");
            imgMock.Setup(thumb => thumb.Prefix).Returns("orginal_");

            var imageConverterMock = new Mock<IImageConverter>();
            imageConverterMock.Setup(ic => ic.ConvertImageToByteArrayAsync(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult(TestHelper.GetSingleImageAsync().Result));
            imageConverterMock.Setup(ic => ic.ConvertImageAsync(TestHelper.GetSingleImageAsync().Result))
                .ReturnsAsync(imgMock.Object);

            var thumbnailConverterMock = new Mock<IThumbnailConverter>();
            thumbnailConverterMock.Setup
                    (tc => tc.GetThumbnailsAsync(TestHelper.GetSingleImageAsync().Result))
                .ThrowsAsync(new ThumbnailConverterException("Thumbnail error", new Exception()));

            var loggerMock = new Mock<ILogger<IImageService>>();

            var dataMock = new Mock<IImageServiceData>();
            dataMock.Setup(fs => fs.FileSystemService.SaveImageAsync("name", new byte[1])).ReturnsAsync(true);
            dataMock.Setup(settings => settings.Settings).Returns(TestHelper.GetImageServiceSettings);
            dataMock.Setup(ic => ic.ImageConverter).Returns(imageConverterMock.Object);
            dataMock.Setup(tc => tc.ThumbnailConverter).Returns(thumbnailConverterMock.Object);

            return new ImageService(dataMock.Object, loggerMock.Object);
        }

        private ImageService Init_ImageService_With_FileSystemService_Exception()
        {
            
            List<IConvertedImage> thumbnails = new List<IConvertedImage>();

            var thumbnailMock = new Mock<IConvertedImage>();
            thumbnailMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            thumbnailMock.Setup(thumb => thumb.Format).Returns("jpeg");
            thumbnailMock.Setup(thumb => thumb.Prefix).Returns("min_");

            var thumbnailMock2 = new Mock<IConvertedImage>();
            thumbnailMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            thumbnailMock.Setup(thumb => thumb.Format).Returns("jpeg");
            thumbnailMock.Setup(thumb => thumb.Prefix).Returns("mid_");

            thumbnails.Add(thumbnailMock.Object);
            thumbnails.Add(thumbnailMock2.Object);

            var imgMock = new Mock<IConvertedImage>();
            imgMock.Setup(thumb => thumb.Image).Returns(TestHelper.GetSingleImageAsync().Result);
            imgMock.Setup(thumb => thumb.Format).Returns("jpeg");
            imgMock.Setup(thumb => thumb.Prefix).Returns("orginal_");

            var fileSystemMock = new Mock<IFileSystemService>();
            fileSystemMock.Setup(fs => fs.SaveImageAsync(It.IsAny<string>(), It.IsAny<byte[]>())).ThrowsAsync(new FileSystemServiceException("FileSystemService error", new Exception()));
            fileSystemMock.Setup(fs => fs.LoadImageAsync(It.IsAny<string>())).ThrowsAsync(new FileSystemServiceException("FileSystemService error", new Exception()));

            var imageConverterMock = new Mock<IImageConverter>();
            imageConverterMock.Setup(ic => ic.ConvertImageToByteArrayAsync(It.IsAny<IFormFile>()))
                .Returns(Task.FromResult(TestHelper.GetSingleImageAsync().Result));
            imageConverterMock.Setup(ic => ic.ConvertImageAsync(TestHelper.GetSingleImageAsync().Result))
                .ReturnsAsync(imgMock.Object);

            var thumbnailConverterMock = new Mock<IThumbnailConverter>();
            thumbnailConverterMock.Setup
                    (tc => tc.GetThumbnailsAsync(TestHelper.GetSingleImageAsync().Result))
                .ReturnsAsync(thumbnails);

            var loggerMock = new Mock<ILogger<IImageService>>();

            var dataMock = new Mock<IImageServiceData>();
            dataMock.Setup(fs => fs.FileSystemService.SaveImageAsync("name", new byte[1])).ReturnsAsync(true);
            dataMock.Setup(settings => settings.Settings).Returns(TestHelper.GetImageServiceSettings);
            dataMock.Setup(ic => ic.ImageConverter).Returns(imageConverterMock.Object);
            dataMock.Setup(tc => tc.ThumbnailConverter).Returns(thumbnailConverterMock.Object);
            dataMock.Setup(fs => fs.FileSystemService).Returns(fileSystemMock.Object);

            return new ImageService(dataMock.Object, loggerMock.Object);
        }

        #endregion

        [Test]
        public async Task UploadImageAsync_Returns_Proper_ImageUploadDto()
        {
            var image = TestHelper.GetMockFormFile();
            var dto = new ImageUploadDto()
            {
                Image = image
            };

            var result = await _imageService.UploadImageAsync(dto);

            Assert.IsInstanceOf(typeof(ImageUploadDto), result);
            Assert.IsTrue(result.IsSucces);
            Assert.IsEmpty(result.ErrorList);
            Assert.IsNotEmpty(result.Id);
            Assert.NotNull(result.Id);
        }

        [Test]
        public async Task DownloadImageAsync_Returns_DownloadedImageDto()
        {
            var dto = new ImageDownloadDto()
            {
                Id = "someId"
            };

            var result = await _imageService.DownloadImageAsync(dto);

            Assert.IsInstanceOf(typeof(ImageDownloadDto), result);
        }

        [Test]
        public async Task DownloadImageAsync_Returns_DownloadedImageDto_Images_Not_Null()
        {
            var dto = new ImageDownloadDto()
            {
                Id = "someId"
            };

            var result = await _imageService.DownloadImageAsync(dto);

            Assert.NotNull(result.OrginalImage);
            Assert.IsNotEmpty(result.Thumbnails);
        }

        [Test]
        public async Task DownloadImageAsync_Returns_DownloadedImageDto_Images_Not_ByteArray0()
        {
            var dto = new ImageDownloadDto()
            {
                Id = "someId"
            };

            var result = await _imageService.DownloadImageAsync(dto);

            Assert.IsNotEmpty(result.OrginalImage.Image);
            result.Thumbnails.Select(t => t.Image).ToList().ForEach(Assert.IsNotEmpty);
        }

        #region Exception Handling Tests
        //////////////////////////////
        // Exception Handling Tests //
        //////////////////////////////

        [Test]
        public async Task UploadImageAsync_On_Image_Converter_Error_Returns_ErrorList_Count_1()
        {
            var service = Init_ImageService_With_ImageConverter_Exception();
            var image = TestHelper.GetMockFormFile();
            var dto = new ImageUploadDto()
            {
                Image = image
            };

            var result = await service.UploadImageAsync(dto);

            Assert.IsNotEmpty(result.ErrorList);
            Assert.IsTrue(result.ErrorList.Count() == 1);
            Assert.AreEqual("Converter error", result.ErrorList.FirstOrDefault());
        }

        [Test]
        public async Task UploadImageAsync_On_ThumbnailConverter_Error_Returns_ErrorList_Count_1()
        {
            var service = Init_ImageService_With_Thumbnail_Exception();
            var image = TestHelper.GetMockFormFile();
            var dto = new ImageUploadDto()
            {
                Image = image
            };

            var result = await service.UploadImageAsync(dto);

            Assert.IsNotEmpty(result.ErrorList);
            Assert.IsTrue(result.ErrorList.Count() == 1);
            Assert.AreEqual("Thumbnail error", result.ErrorList.FirstOrDefault());
        }

        [Test]
        public async Task UploadImageAsync_On_FileSystemService_Error_Returns_ErrorList_Count_1()
        {
            var service = Init_ImageService_With_FileSystemService_Exception();
            var image = TestHelper.GetMockFormFile();
            var dto = new ImageUploadDto()
            {
                Image = image
            };

            var result = await service.UploadImageAsync(dto);

            Assert.IsNotEmpty(result.ErrorList);
            Assert.IsTrue(result.ErrorList.Count() == 1);
            Assert.AreEqual("FileSystemService error", result.ErrorList.FirstOrDefault());
        }

        [Test]
        public async Task DownloadImageAsync_On_FileSystemService_Error_Returns_ErrorList_Count_1()
        {
            var service = Init_ImageService_With_FileSystemService_Exception();
            var dto = new ImageDownloadDto()
            {
                Id = "someId"
            };

            var result = await service.DownloadImageAsync(dto);

            Assert.IsNotEmpty(result.ErrorList);
            Assert.IsTrue(result.ErrorList.Count() == 1);
            Assert.AreEqual("FileSystemService error", result.ErrorList.FirstOrDefault());
        }
        #endregion
    }
}
