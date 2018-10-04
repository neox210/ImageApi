using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;

namespace ImageApi.Settings
{
    public class ConversionSettings
    {
        public ThumbnailConverterSettings ThumbnailConverterSettings { get; set; }
        public ImageConverterSettings ImageConverterSettings { get; set; }
    }
}
