using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Model;
using Microsoft.AspNetCore.Http;

namespace ImageApi.Interfaces
{
    public interface IImageConverter
    {
        Task<IConvertedImage> ConvertImageAsync(byte[] image);
        Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image);
    }
}
