using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interface
{
    public interface IFileManagerAPI
    {
        Task CreateFileAsync(string targetPath, string fileName, IFormFile file);
        Task<byte[]> GetFileAsync(string targetPath, string fileName);
        Task DeleteFileAsync(string targetPath, string fileName);

    }
}
