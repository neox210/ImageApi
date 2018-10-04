using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using ImageApi.Settings;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json.Linq;

namespace ImageApi_Tests.TestFiles
{
    /// <summary>
    /// Helper Class for test file retrieval
    /// </summary>
    public static class TestHelper
    {
        public static async Task<byte[]> GetSingleImageAsync()
        {
            byte[] image;
            using (var stream = EmbededData.AsStream("ImagesJpg.1.jpg"))
            {
                image = new byte[stream.Length];
                await stream.ReadAsync(image, 0, (int) stream.Length);
            }

            return image;
        }

        public static async Task<IEnumerable<byte[]>> GetMultipleImagesAsync()
        {
            List<byte[]> imagesList = new List<byte[]>();

            using (var stream1 = EmbededData.AsStream("ImagesJpg.1.jpg"))
            using (var stream2 = EmbededData.AsStream("ImagesJpg.2.jpg"))
            using (var stream3 = EmbededData.AsStream("ImagesJpg.3.jpg"))
            using (var stream4 = EmbededData.AsStream("ImagesJpg.4.jpg"))
            {
                var image1 = new byte[stream1.Length];
                var image2 = new byte[stream2.Length];
                var image3 = new byte[stream3.Length];
                var image4 = new byte[stream4.Length];

                await stream1.ReadAsync(image1, 0, (int)stream1.Length).ContinueWith(i => imagesList.Add(image1));
                await stream2.ReadAsync(image2, 0, (int)stream2.Length).ContinueWith(i => imagesList.Add(image2));
                await stream3.ReadAsync(image3, 0, (int)stream3.Length).ContinueWith(i => imagesList.Add(image3));
                await stream4.ReadAsync(image4, 0, (int)stream4.Length).ContinueWith(i => imagesList.Add(image4));
            }

            return imagesList;
        }

        public static async Task<IDictionary<string, byte[]>> GetThumbnail()
        {
            var thumb = new KeyValuePair<string, byte[]>("thumb_", await GetSingleImageAsync());
            IDictionary<string, byte[]> dict = new Dictionary<string, byte[]>();
            dict.Add(thumb);
            return dict;
        }

        public static ISettings GetImageServiceSettings()
        {
            return JObject.Parse(EmbededData.AsString("Settings.testSettings.json"))
                .SelectToken("Settings")
                .ToObject<Settings>();
        }

        public static IFormFile GetMockFormFile()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "TestFiles\\ImagesJpg\\1.jpeg");
            var physicalFile = new FileInfo(path);
            var fileMock = new Mock<IFormFile>();
            var ms = new MemoryStream();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
            fileMock.Setup(m => m.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));

            ms.Flush();
            ms.Close();
            return fileMock.Object;
        }
    }
}
