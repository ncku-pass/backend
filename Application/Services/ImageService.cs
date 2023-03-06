using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IFileManagerAPI _fileManagerAPI;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _imageFolderPath;
        private readonly int _userId;

        public ImageService(
            IHttpContextAccessor httpContextAccessor,
            IFileManagerAPI fileManagerAPI,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            this._fileManagerAPI = fileManagerAPI;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;

            try
            {
                this._imageFolderPath = Path.Combine(httpContextAccessor.HttpContext.User.FindFirst("studentId").Value, "images");
                this._userId = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                this._userId = 1;
                this._imageFolderPath = Path.Combine("error", "images");
            }
        }

        public async Task<ImageFileResponse> GetImageAsync(int imageId)
        {
            var imageModel = await this._unitOfWork.Image.SingleOrDefaultAsync(img => img.Id == imageId && img.UserId == this._userId);
            var imageName = imageModel.Name + "." + imageModel.Extension;

            var imageResponse = new ImageFileResponse
            {
                Name = imageName,
                ImageBytes = await _fileManagerAPI.GetFileAsync(this._imageFolderPath, imageName)
            };
            return imageResponse;
        }


        public async Task<List<ImageResponse>> UploadImageAsync(List<IFormFile> files)
        {
            var imageModels = new List<Image> { };

            foreach (var item in files)
            {
                string fileName = GenerateRandomString(7);
                string extension = Path.GetExtension(item.FileName);
                await _fileManagerAPI.CreateFileAsync(this._imageFolderPath, fileName + extension, item);

                var imageModel = new Image
                {
                    Name = fileName,
                    Extension = extension.Replace(".", ""),
                    UserId = this._userId
                };
                this._unitOfWork.Image.Add(imageModel);
                await this._unitOfWork.SaveChangeAsync();

                imageModels.Add(imageModel);
            }

            var imageResponses = this._mapper.Map<List<ImageResponse>>(imageModels);

            return imageResponses;
        }
        static string GenerateRandomString(int length)
        {
            const string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var result = new string(Enumerable.Range(0, length)
                                              .Select(i => chars[random.Next(chars.Length)])
                                              .ToArray());
            return result;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var imageModel = await this._unitOfWork.Image.SingleOrDefaultAsync(img => img.Id == imageId && img.UserId == this._userId);
            await _fileManagerAPI.DeleteFileAsync(this._imageFolderPath, imageModel.Name + "." + imageModel.Extension);
            this._unitOfWork.Image.Remove(imageModel);
            await this._unitOfWork.SaveChangeAsync();
        }


    }
}
