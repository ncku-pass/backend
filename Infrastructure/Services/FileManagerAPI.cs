using Infrastructure.Services.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class FileManagerAPI : IFileManagerAPI
    {
        async Task<bool> IFileManagerAPI.CreateFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        Task<bool> IFileManagerAPI.DeleteFileAsync(string path)
        {
            throw new System.NotImplementedException();
        }

        Task<IFormFile> IFileManagerAPI.ReadFileAsync(string path)
        {
            throw new System.NotImplementedException();
        }
    }
}
