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

        Task<ImageResponse> UploadImageAsync(IFormFile file);

        Task DeleteImageAsync(int imageId);

        Task ImgExistAsync(int[] imgIds);
    }
}