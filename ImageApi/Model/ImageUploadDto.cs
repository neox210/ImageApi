using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ImageApi.Model
{
    public class ImageUploadDto : ImageDto
    {
        public IFormFile Image { get; set; }
    }
}
