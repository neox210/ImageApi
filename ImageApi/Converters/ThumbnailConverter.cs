using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Settings;
using ImageMagick;

namespace ImageApi.Converters
{
    /// <summary>
    /// Creates Images Thumbnails.
    /// </summary>
    public class ThumbnailConverter : IThumbnailConverter
    {
        private readonly ThumbnailConverterSettings _settings;

        public ThumbnailConverter(ISettings settings)
        {
            _settings = settings.ImageServiceSettings.ConversionSettings.ThumbnailConverterSettings;
        }
        /// <summary>
        /// Async Gets Images Thumbnails from byte array.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IConvertedImage>> GetThumbnailsAsync(byte[] image)
        {
            try
            {
                return await TryGetThumbnailsAsync(image);
            }
            catch (MagickCorruptImageErrorException e)
            {
                throw new ThumbnailConverterException($"ImageMagick Error: Corrupted image. {e.Message}", e);
            }
            catch (MagickException e)
            {
                throw new ThumbnailConverterException($"ImageMagickError occurs when creating a thumbnail: {e.Message}", e);
            }
            catch (Exception e)
            {
                throw new ThumbnailConverterException("Unexpected Error occurs when creating a thumbnail", e);
            }

        }

        private async Task<IEnumerable<IConvertedImage>> TryGetThumbnailsAsync(byte[] image)
        {
            var thumbnails = new List<IConvertedImage>();

            var thumbnailsTask = new Task(() =>
                {
                    thumbnails.AddRange(_settings.Thumbnails.Select(thumb => GetSingleThumbnail(image, thumb)));
                }
            );

            thumbnailsTask.Start();
            await thumbnailsTask;

            return thumbnails;
        }

        private IConvertedImage GetSingleThumbnail(byte[] image, Thumbnail thumbnailSettings)
        {
            byte[] thumbnailImage;
           
            using (var mgkImage = new MagickImage(image))
            {
                mgkImage.Resize(GetMagickSize(thumbnailSettings));
                mgkImage.Quality = thumbnailSettings.Quality;
                mgkImage.Format = SettingsHelper.GetFormat(thumbnailSettings.Format);
                thumbnailImage = mgkImage.ToByteArray(SettingsHelper.GetFormat(thumbnailSettings.Format));
            }
            var thumbnail = new ConvertedImage()
            {
                Image = thumbnailImage,
                Prefix = thumbnailSettings.Prefix,
                Format = thumbnailSettings.Format
            };

            return thumbnail;
        }

        private MagickGeometry GetMagickSize(Thumbnail thumbnailSettings)
        {
            return new MagickGeometry(thumbnailSettings.Width, thumbnailSettings.Height)
            {
                IgnoreAspectRatio = _settings.IgnoreAspectRatio
            };
        }

        
    }
}
