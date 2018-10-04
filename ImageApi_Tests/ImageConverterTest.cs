using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageApi.Converters;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi_Tests.TestFiles;
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ImageApi_Tests
{
    [TestFixture]
    public class ImageConverterTest
    {
        private readonly IImageConverter _imageConverter;
        public ImageConverterTest()
        {
            var settings = TestHelper.GetImageServiceSettings();
            _imageConverter = new ImageConverter(settings);
        }

        [Test]
        public async Task ConvertImageAsync_Returns_Poper_Prefix()
        {
            byte[] image = await TestHelper.GetSingleImageAsync();

            var result = await _imageConverter.ConvertImageAsync(image);

            Assert.IsTrue(result.Prefix == "orginal_");
        }

        [Test]
        public async Task ConvertImageAsync_Returns_Poper_Format()
        {
            byte[] image = await TestHelper.GetSingleImageAsync();

            var result = await _imageConverter.ConvertImageAsync(image);

            Assert.IsTrue(result.Format == "jpeg");
        }

        [Test]
        public async Task ConvertImageAsync_Returns_ConvertedImage()
        {
            byte[] image = await TestHelper.GetSingleImageAsync();

            var result = await _imageConverter.ConvertImageAsync(image);

            Assert.IsInstanceOf(typeof(ConvertedImage), result);
        }

        [Test]
        public async Task ConvertImageToByteArrayAsync_Returns_Byte_Array()
        {
            IFormFile file = TestHelper.GetMockFormFile();

            var result = await _imageConverter.ConvertImageToByteArrayAsync(file);

            Assert.IsInstanceOf(typeof(byte[]), result);
        }
    }
}
