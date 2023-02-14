using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Infrastructure.Services.Interface
{
    public interface IFileManagerAPI
    {
        Task<bool> CreateFileAsync(IFormFile file);
        Task<IFormFile> ReadFileAsync(string path);
        Task<bool> DeleteFileAsync(string path);
    }
}
