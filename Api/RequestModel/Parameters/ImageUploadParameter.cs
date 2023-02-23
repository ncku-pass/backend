using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class ImageUploadParameter
    {
        [Required]
        public List<IFormFile> ImageFiles { get; set; }
    }
}
