using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;

namespace ImageApi.Settings
{
    public class ThumbnailConverterSettings
    {
        public IEnumerable<Thumbnail> Thumbnails { get; set; }
        public bool IgnoreAspectRatio { get; set; }
    }
}
