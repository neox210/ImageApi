using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageApi.Exceptions;
using ImageApi.Interfaces;
using ImageApi.Settings;

namespace ImageApi.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly FileSystemServiceSettings _settings;
        public FileSystemService(ISettings settings)
        {
            _settings = settings.FileSystemServiceSettings;
        }

        public async Task<bool> SaveImageAsync(string name, byte[] image)
        {
            try
            {
                return await TrySaveImageAsync(name, image);
            }
            catch (Exception e)
            {
                throw new FileSystemServiceException($"error occurs in FileSystemService when saving an imageName: {name} in directory: {_settings.ImageDirectory}", e);
            }
        }

        private async Task<bool> TrySaveImageAsync(string name, byte[] image)
        {
            CreateDirectory();
            string path = Path.Combine(_settings.ImageDirectory, name);
            await File.WriteAllBytesAsync(path, image);
            return true;
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(_settings.ImageDirectory))
            {
                Directory.CreateDirectory(_settings.ImageDirectory);
            }
        }

        public async Task<byte[]> LoadImageAsync(string imageName)
        {
            try
            {
                return await TryLoadImageAsync(imageName);
            }
            catch (FileNotFoundException e)
            {
                throw new FileSystemServiceException($"File not found imageName: {imageName} in directory: {_settings.ImageDirectory}",e);
            }
            catch (Exception e)
            {
                throw new FileSystemServiceException($"error occurs in FileSystemService when loading an imageName: {imageName} from directory: {_settings.ImageDirectory}", e);
            }
            
        }

        private async Task<byte[]> TryLoadImageAsync(string imageName)
        {
            string path = Path.Combine(_settings.ImageDirectory, imageName);
            return await File.ReadAllBytesAsync(path);
        }
    }
}
