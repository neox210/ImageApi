using System;
using System.IO;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Settings;
using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace ImageApi.Converters
{
    public class ImageConverter : IImageConverter
    {
        private readonly ImageConverterSettings _settings;
        public ImageConverter(ISettings settings)
        {
            _settings = settings.ImageServiceSettings.ConversionSettings.ImageConverterSettings;
        }

        public async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image)
        {
            try
            {
                return await TryConvertImageToByteArrayAsync(image);
            }
            catch (Exception e)
            {
                throw new ImageConverterException("Unexpected Error occurs when converting image to byte array", e);
            }
        }

        private static async Task<byte[]> TryConvertImageToByteArrayAsync(IFormFile image)
        {
            byte[] newImage;
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                newImage = ms.ToArray();
            }

            return newImage;
        }

        public async Task<IConvertedImage> ConvertImageAsync(byte[] image)
        {
            try
            {
                return await TryConvertImageAsync(image);
            }
            catch (MagickCorruptImageErrorException e)
            {
                throw new ImageConverterException($"ImageMagick Error: Corrupted image. {e.Message}", e);
            }
            catch (MagickException e)
            {
                throw new ImageConverterException($"ImageMagickError occurs when converting orginal image: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new ImageConverterException("Unexpected Error occurs when converting orginal image", e);
            }
        }

        private async Task<IConvertedImage> TryConvertImageAsync(byte[] image)
        {
            IConvertedImage newImage = new ConvertedImage();

            var task = new Task(() => newImage = ConvertImage(image));
            task.Start();
            await task;
            return newImage;
        }

        private IConvertedImage ConvertImage(byte[] image)
        {
            byte[] newImage;

            using (var mgkImage = new MagickImage(image))
            {
                mgkImage.Quality = _settings.Quality;
                mgkImage.Format = SettingsHelper.GetFormat(_settings.Format);
                newImage = mgkImage.ToByteArray();
            }

            var img = new ConvertedImage()
            {
                Image = newImage,
                Prefix = _settings.Prefix,
                Format = _settings.Format
            };

            return img;
        }


    }
}
