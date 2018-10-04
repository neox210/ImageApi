using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using ImageApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageApi.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ImageController : Controller
    {
        private IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(ImageUploadDto imageDto)
        {
            var isImageUpdated = await _imageService.UploadImageAsync(imageDto);

            return Ok(isImageUpdated);
        }

        [HttpPost]
        public async Task<IActionResult> Download([FromBody] ImageDownloadDto imageDto)
        {
            var image = await _imageService.DownloadImageAsync(imageDto);

            return Ok(image);
        }
    }
}