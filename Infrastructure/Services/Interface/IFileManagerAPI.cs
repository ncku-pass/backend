using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interface
{
    public interface IFileManagerAPI
    {
        Task<bool> CreateFileAsync(string targetPath, string fileName, IFormFile file);
        Task<byte[]> GetFileAsync(string targetPath, string fileName);
        Task<bool> DeleteFileAsync(string targetPath, string fileName);

    }
}
