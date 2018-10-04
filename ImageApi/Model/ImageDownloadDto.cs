using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;

namespace ImageApi.Model
{
    public class ImageDownloadDto : ImageDto
    {
        public IConvertedImage OrginalImage { get; set; }
        public IEnumerable<IConvertedImage> Thumbnails { get; set; }
    }
}
