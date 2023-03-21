using Api.RequestModel.Parameters.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class ImageUploadParameter
    {
        [Required]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [AllowFileSize(5 * 1024 * 1024)]
        public IFormFile Image { get; set; }
    }
}