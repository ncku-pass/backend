using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly IFileManagerAPI _fileManagerAPI;
        private readonly IAESCryptAPI _AESCryptAPI;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly string _imageFolderPath;
        private readonly int _userId;

        public ImageService(
            IHttpContextAccessor httpContextAccessor,
            IFileManagerAPI fileManagerAPI,
            IAESCryptAPI aESCryptAPI,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            this._fileManagerAPI = fileManagerAPI;
            this._AESCryptAPI = aESCryptAPI;
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

        public async Task<ImageFileResponse> GetImageAsync(int imageId, string imageToken)
        {
            var studentId = this._AESCryptAPI.Decrypt(imageToken);
            if (!Regex.IsMatch(studentId, @"[a-zA-Z]([a-zA-Z0-9])\d{7}"))
            {
                throw new InvalidOperationException($"Invalid Token.");
            }

            var imageModel = await this._unitOfWork.Image.SingleOrDefaultAsync(img => img.Id == imageId);
            if (imageModel == null)
            {
                throw new InvalidOperationException($"Image with ID {imageId} does not exist.");
            }
            var imageName = imageModel.Name + "." + imageModel.Extension;

            return new ImageFileResponse
            {
                Name = imageName,
                ImageBytes = await _fileManagerAPI.GetFileAsync(Path.Combine(studentId, "images"), imageName)
            };
        }

        public async Task<List<ImageResponse>> GetExperienceImageAsync(int expId)
        {
            var imgIds = await this._unitOfWork.Experience_Image.Where(ei => ei.ExperienceId == expId).Select(ei => ei.ImageId).ToListAsync();
            var imgModel = await this._unitOfWork.Image.Where(i => imgIds.Contains(i.Id)).ToListAsync();

            var imgResponse = this._mapper.Map<List<ImageResponse>>(imgModel);
            return imgResponse;
        }

        public async Task<ImageResponse> UploadImageAsync(IFormFile file)
        {
            string fileName = this._AESCryptAPI.GenerateRandomString(7);
            string extension = Path.GetExtension(file.FileName);
            await _fileManagerAPI.CreateFileAsync(this._imageFolderPath, fileName + extension, file);

            var imageModel = new Image
            {
                Name = fileName,
                Extension = extension.Replace(".", ""),
                Size = file.Length,
                UserId = this._userId
            };
            this._unitOfWork.Image.Add(imageModel);
            await this._unitOfWork.SaveChangeAsync();

            var imageResponse = this._mapper.Map<ImageResponse>(imageModel);
            return imageResponse;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var imageModel = await this._unitOfWork.Image.SingleOrDefaultAsync(img => img.Id == imageId && img.UserId == this._userId);
            await _fileManagerAPI.DeleteFileAsync(this._imageFolderPath, imageModel.Name + "." + imageModel.Extension);
            this._unitOfWork.Image.Remove(imageModel);
            await this._unitOfWork.SaveChangeAsync();
        }

        public async Task ImgExistAsync(List<int> imgIds)
        {
            var userImgsList = await _unitOfWork.Image.Where(t => t.UserId == this._userId).Select(t => t.Id).ToListAsync();
            var notExistImgsIds = imgIds.Except(userImgsList).ToList();

            if (notExistImgsIds.Any())
            {
                throw new ArgumentException($"查無此Imgs=>Ids:{string.Join(", ", notExistImgsIds)}");
            }
        }
    }
}