using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileManagerAPI : IFileManagerAPI
    {
        private readonly string _uploadFolderPath;

        public FileManagerAPI()
        {
            _uploadFolderPath = Environment.GetEnvironmentVariable("UPLOAD_PATH");
        }

        public async Task CreateFileAsync(string targetPath, string fileName, IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(_uploadFolderPath, targetPath));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    throw new ArgumentException("File is Empty.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        // 讀取檔案
        public async Task<byte[]> GetFileAsync(string targetPath, string fileName)
        {
            var filePath = Path.Combine(_uploadFolderPath, targetPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not Found.");
            }

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);
                    return ms.ToArray();
                }
            }
        }

        public async Task DeleteFileAsync(string targetPath, string fileName)
        {
            var filePath = Path.Combine(_uploadFolderPath, targetPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not Found.");
            }

            try
            {
                await Task.Run(() => File.Delete(filePath));
            }
            catch (Exception ex)
            {
                throw new Exception("File Delete Failed", ex);
            }
        }
    }
}