using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using Microsoft.Extensions.Logging;

namespace ImageApi.Model
{
    public class ImageServiceData : IImageServiceData
    {
        public ISettings Settings { get; set; }
        public IThumbnailConverter ThumbnailConverter { get; set; }
        public IFileSystemService FileSystemService { get; set; }
        public IImageConverter ImageConverter { get; set; }

        public ImageServiceData(  ISettings settings
                                , IThumbnailConverter thumbnailConverter
                                , IFileSystemService fileSystemService
                                , IImageConverter imageConverter
                               )
        {
            Settings = settings;
            ThumbnailConverter = thumbnailConverter;
            FileSystemService = fileSystemService;
            ImageConverter = imageConverter;
        }
    }
}
