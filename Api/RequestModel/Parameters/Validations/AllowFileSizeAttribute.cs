using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters.Validations
{
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public AllowFileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            foreach (var file in files)
            {
                if (file.Length > _maxSize)
                {
                    return new ValidationResult($"以下檔案大小超過上限 {_maxSize / (1024 * 1024)} MB：{file.FileName}");
                }
            }
            return ValidationResult.Success;
        }
    }
}