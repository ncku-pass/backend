using Microsoft.AspNetCore.Http;
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
            var file = value as IFormFile;

            if (file.Length > _maxSize)
            {
                return new ValidationResult($"{file.FileName} 檔案大小超過上限 {_maxSize / (1024 * 1024)} MB");
            }

            return ValidationResult.Success;
        }
    }
}