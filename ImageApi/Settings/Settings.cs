using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ImageApi.Settings
{
    public class Settings : ISettings
    {
        public FileSystemServiceSettings FileSystemServiceSettings { get; set; }
        public ImageServiceSettings ImageServiceSettings { get; set; }

        public Settings()
        {
            
        }

        public Settings(IConfiguration configuration)
        {
            var settingsSection = configuration.GetSection("Settings");
            var settings = settingsSection.Get<Settings>();

            FileSystemServiceSettings = settings.FileSystemServiceSettings;
            ImageServiceSettings = settings.ImageServiceSettings;
        }
    }
}
