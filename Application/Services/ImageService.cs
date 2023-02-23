using Application.Services.Interface;
using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageFolderPath;
        private readonly IFileManagerAPI _fileManagerAPI;

        public ImageService(
            IHttpContextAccessor httpContextAccessor,
            IFileManagerAPI fileManagerAPI
            )
        {
            var studentId = httpContextAccessor.HttpContext.User.FindFirst("studentId").Value;
            this._imageFolderPath = Path.Combine(studentId, "images");
            this._fileManagerAPI = fileManagerAPI;
        }


        public async Task<List<string>> UploadImageAsync(List<IFormFile> files)
        {
            var nameList = new List<string> { };

            foreach (var item in files)
            {
                string fileName = GenerateRandomString(7) + Path.GetExtension(item.FileName);
                await _fileManagerAPI.CreateFileAsync(this._imageFolderPath, fileName, item);
                nameList.Add(fileName);
            }

            return nameList;
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
        public async Task<byte[]> GetImageAsync(string fileName)
        {
            return await _fileManagerAPI.GetFileAsync(this._imageFolderPath, fileName);
        }

        public async Task DeleteImageAsync(string fileName)
        {
            await _fileManagerAPI.DeleteFileAsync(this._imageFolderPath, fileName);
        }


    }
}
