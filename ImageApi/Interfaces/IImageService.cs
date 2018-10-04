using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace ImageApi.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadDto> UploadImageAsync(ImageUploadDto imageDto);
        Task<ImageDownloadDto> DownloadImageAsync(ImageDownloadDto imageDto);
    }
}
