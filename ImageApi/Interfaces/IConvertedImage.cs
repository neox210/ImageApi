using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageApi.Interfaces
{
    public interface IConvertedImage
    {
        byte[] Image { get; set; }
        string Prefix { get; set; }
        string Format { get; set; }
    }
}
