using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using ImageMagick;

namespace ImageApi.Model
{
    public class ConvertedImage : IConvertedImage
    {
        public byte[] Image { get; set; }
        public string Prefix { get; set; }
        public string Format { get; set; }
    }
}
