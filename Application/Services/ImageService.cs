using Application.Services.Interface;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        public ImageService()
        {
        }

        Task<string> IImageService.UploadImageAsync()
        {
            throw new NotImplementedException();
        }

        Task<string> IImageService.DeleteImageAsync()
        {
            throw new NotImplementedException();
        }

        Task<string> IImageService.GetImageAsync()
        {
            throw new NotImplementedException();
        }

    }
}
