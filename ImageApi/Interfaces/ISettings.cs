using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Settings;

namespace ImageApi.Interfaces
{
    public interface ISettings
    {
        FileSystemServiceSettings FileSystemServiceSettings { get; set; }
        ImageServiceSettings ImageServiceSettings { get; set; }
    }
}
