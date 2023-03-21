using Api.RequestModel.Parameters.Validations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class ImageUploadParameter
    {
        [Required]
        [AllowedExtensions(new string[] { ".jpg", ".jpeg", ".png", ".gif" })]
        [AllowFileSize(5 * 1024 * 1024)]
        public List<IFormFile> ImageFiles { get; set; }
    }
}