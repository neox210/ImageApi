using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Model;
using ImageApi.Settings;
using Microsoft.Extensions.Logging;

namespace ImageApi.Services
{
    public class ImageService : IImageService
    {
        private readonly ImageServiceSettings _settings;
        private readonly IThumbnailConverter _thumbnailConverter;
        private readonly IImageConverter _imageConverter;
        private readonly IFileSystemService _fileSystemService;
        private readonly ILogger _logger;

        public ImageService(IImageServiceData data, ILogger<IImageService> logger)
        {
            _settings = data.Settings.ImageServiceSettings;
            _thumbnailConverter = data.ThumbnailConverter;
            _fileSystemService = data.FileSystemService;
            _imageConverter = data.ImageConverter;
            _logger = logger;
        }

        public async Task<ImageUploadDto> UploadImageAsync(ImageUploadDto imageDto)
        {
            try
            {
                imageDto.IsSucces = await TryUploadImageAsync(imageDto);
                return imageDto;
            }
            catch (ImageConverterException e)
            {
                return SetErrorList(imageDto, e) as ImageUploadDto;
            }
            catch (ThumbnailConverterException e)
            {
                return SetErrorList(imageDto, e) as ImageUploadDto;
            }
            catch (FileSystemServiceException e)
            {
                return SetErrorList(imageDto, e)as ImageUploadDto;
            }
            catch (Exception e)
            {
                return SetErrorList(imageDto, e) as ImageUploadDto;
            }
        }

        public async Task<ImageDownloadDto> DownloadImageAsync(ImageDownloadDto imageDto)
        {
            try
            {
                return await TryDownloadImageAsync(imageDto);
            }
            catch (FileSystemServiceException e)
            {
                return SetErrorList(imageDto, e) as ImageDownloadDto;
            }
            catch (Exception e)
            {
                return SetErrorList(imageDto, e) as ImageDownloadDto;
            }
        }

        private IErrorable SetErrorList(ImageDto imageDto, Exception e)
        {
            _logger.LogError(e, e.Message);
            imageDto.IsSucces = false;

            if (imageDto.ErrorList == null)
            {
                imageDto.ErrorList = new List<string>()
                {
                    e.Message
                };
            }
            else
            {
                imageDto.ErrorList.Add(e.Message);
            }

            return imageDto;
        }

        #region UploadMethods

        private async Task<bool> TryUploadImageAsync(ImageUploadDto imageDto)
        {
            imageDto.Id = Guid.NewGuid().ToString("N");

            _logger.LogInformation($"Sterting ImageConversion imageId: {imageDto.Id}");
            var orginalImage = await _imageConverter.ConvertImageToByteArrayAsync(imageDto.Image);
            var convertedOrginalImage = _imageConverter.ConvertImageAsync(orginalImage);
            var thumbnails = _thumbnailConverter.GetThumbnailsAsync(orginalImage);

            var isOrginalImageUploaded = UploadSingleImageImageAsync(await convertedOrginalImage, imageDto.Id);
            var areThumbnailsUpdated =  UploadThumbnailsAsync(await thumbnails, imageDto.Id);

            var result = await isOrginalImageUploaded && await areThumbnailsUpdated;
            string msg = result ? "successed" : "failed";
            _logger.LogInformation($"ImageID : {imageDto.Id} conversion {msg}");
            return result;
        }

        private async Task<bool> UploadSingleImageImageAsync(IConvertedImage convertedImage, string imageId)
        {
            string fullName = string.Concat(convertedImage.Prefix, imageId, '.', convertedImage.Format);
            return await _fileSystemService.SaveImageAsync(fullName, convertedImage.Image);
        }

        private async Task<bool> UploadThumbnailsAsync(IEnumerable<IConvertedImage> thumbnails, string imageId)
        {
            List<bool> areThumbnailsSavedList = new List<bool>();

            foreach (var thumbnail in thumbnails)
            {
                bool isThumbnailSaved = await UploadSingleImageImageAsync(thumbnail, imageId);
                areThumbnailsSavedList.Add(isThumbnailSaved);
            }

            return areThumbnailsSavedList.All(t => true);
        }

        #endregion

        #region Download Methods

        private async Task<ImageDownloadDto> TryDownloadImageAsync(ImageDownloadDto imageDto)
        {
            _logger.LogInformation($"Starting download Image: {imageDto.Id}");
            var orginalImage = GetOrginalImage(imageDto.Id);
            var thumbnails = GetThumbnails(imageDto.Id);

            var result = new ImageDownloadDto()
            {
                OrginalImage = await orginalImage,
                Thumbnails = await thumbnails
            };

            _logger.LogInformation($"Download complete for Image: {imageDto.Id}");

            return result;
        }

        private async Task<IConvertedImage> GetOrginalImage(string id)
        {
            var prefix = _settings.ConversionSettings.ImageConverterSettings.Prefix;
            var format = _settings.ConversionSettings.ImageConverterSettings.Format;
            var fullId = string.Concat(prefix, id, '.', format);

            return await GetSingleImage(fullId);
        }

        private async Task<IConvertedImage> GetSingleImage(string fullId)
        {
            var imgByteArray = _fileSystemService.LoadImageAsync(fullId);
            var image = new ConvertedImage()
            {
                Image = await imgByteArray
            };

            return image;
        }

        private async Task<IEnumerable<IConvertedImage>> GetThumbnails(string id)
        {
            var thumbnails = new List<IConvertedImage>();

            var thumbnailSettings = _settings.ConversionSettings.ThumbnailConverterSettings.Thumbnails;
            foreach (var thumbnail in thumbnailSettings)
            {
                var fullId = string.Concat(thumbnail.Prefix, id, '.', thumbnail.Format);
                var image = await GetSingleImage(fullId);
                thumbnails.Add(image);
            }

            return thumbnails;
        }

        #endregion

    }
}
