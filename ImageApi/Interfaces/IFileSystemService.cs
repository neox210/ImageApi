using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageApi.Interfaces
{
    public interface IFileSystemService
    {
        Task<bool> SaveImageAsync(string name, byte[] image);
        Task<byte[]> LoadImageAsync(string imageName);
    }
}
