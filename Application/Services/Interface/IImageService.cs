using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IImageService
    {
        Task<string> UploadImageAsync();
        Task<string> GetImageAsync();
        Task<string> DeleteImageAsync();
    }
}
