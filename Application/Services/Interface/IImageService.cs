using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IImageService
    {
        Task<List<string>> UploadImageAsync(List<IFormFile> files);
        Task<byte[]> GetImageAsync(string fileName);
        Task DeleteImageAsync(string fileName);
    }
}
