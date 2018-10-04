using ImageMagick;
using Microsoft.Extensions.Configuration;

namespace ImageApi.Settings
{
    public static class SettingsHelper
    {
        public static MagickFormat GetFormat(string formatName)
        {
            switch (formatName.ToLower().Trim())
            {
                case "bmp":
                    return MagickFormat.Bmp;
                case "jpeg":
                    return MagickFormat.Jpeg;
                case "gif":
                    return MagickFormat.Gif;
                case "png":
                    return MagickFormat.Png;
                case "tiff":
                    return MagickFormat.Tiff;
                case "tif":
                    return MagickFormat.Tiff;
                default:
                    return MagickFormat.Jpeg;
            }
        }
    }
}

