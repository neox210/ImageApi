using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ImageApi.Interfaces
{
    public interface IImageServiceData
    {
        ISettings Settings { get; set; }
        IThumbnailConverter ThumbnailConverter { get; set; }
        IFileSystemService FileSystemService { get; set; }
        IImageConverter ImageConverter { get; set; }
    }
}
