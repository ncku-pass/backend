using Application.Dto.Responses;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IImageService
    {
        Task<ImageFileResponse> GetImageAsync(int imageId);
        Task<ImageFileResponse> GetImageAsync(string imageName);
        Task<List<ImageResponse>> GetExperienceImageAsync(int expId);
        Task<List<ImageResponse>> UploadImageAsync(List<IFormFile> files);
        Task DeleteImageAsync(int imageId);
    }
}
