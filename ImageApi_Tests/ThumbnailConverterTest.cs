using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ImageApi.Converters;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Settings;
using ImageApi_Tests.TestFiles;
using ImageMagick;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace ImageApi_Tests
{
    [TestFixture]
    public class ThumbnailConverterTest
    {
        private readonly IThumbnailConverter _thumbnailConverter;

        public ThumbnailConverterTest()
        {
            var settings = TestHelper.GetImageServiceSettings();
            _thumbnailConverter = new ThumbnailConverter(settings);
        }

        [Test]
        public async Task GetThumbnails_Returns_IEnumerable_Of_ConvertedImage()
        {
            byte[] image = TestHelper.GetSingleImageAsync().Result;

            var result = await _thumbnailConverter.GetThumbnailsAsync(image);

            Assert.IsInstanceOf(typeof(IEnumerable<IConvertedImage>), result);
        }

        [Test]
        public async Task GetThumbnails_Returns_3_Thumbnails_200x100_300x150_500x200()
        {
            byte[] image = TestHelper.GetSingleImageAsync().Result;

            var result = await _thumbnailConverter.GetThumbnailsAsync(image);
            
            using (var mgkImage1 = new MagickImage(result.Single(i => i.Prefix == "mini_").Image))
            using (var mgkImage2 = new MagickImage(result.Single(i => i.Prefix == "mid_").Image))
            using (var mgkImage3 = new MagickImage(result.Single(i => i.Prefix == "max_").Image))
            {
                Assert.True(mgkImage1.Width == 200 || mgkImage1.Height == 100);
                Assert.True(mgkImage2.Width == 300 || mgkImage2.Height == 150);
                Assert.True(mgkImage3.Width == 500 || mgkImage3.Height == 200);
            }
        }

        [Test]
        public async Task GetThumbnails_Returns_Proper_Keys_In_Dict()
        {
            byte[] image = TestHelper.GetSingleImageAsync().Result;

            var result = await _thumbnailConverter.GetThumbnailsAsync(image);

            var convertedImages = result.ToList();
            Assert.IsTrue(convertedImages.Any(i => i.Prefix == "mini_"));
            Assert.IsTrue(convertedImages.Any(i => i.Prefix == "mid_"));
            Assert.IsTrue(convertedImages.Any(i => i.Prefix == "max_"));
        }

        [Test]
        public async Task GetThumbnails_Returns_JPEG()
        {
            byte[] image = TestHelper.GetSingleImageAsync().Result;

            var result = await _thumbnailConverter.GetThumbnailsAsync(image);

            using (var mgkImage1 = new MagickImage(result.First().Image))
            {
                Assert.True(mgkImage1.Format == MagickFormat.Jpeg);
            }
            Assert.IsTrue(result.All(i => i.Format == "jpeg"));
        }
    }
}
