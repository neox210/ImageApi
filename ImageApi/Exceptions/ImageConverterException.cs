using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageApi.Exceptions
{
    public class ImageConverterException : Exception
    {
        public ImageConverterException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
