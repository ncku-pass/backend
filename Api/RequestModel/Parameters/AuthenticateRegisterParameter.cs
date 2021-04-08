using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class AuthenticateRegisterParameter
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "密碼輸入不一致")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string StudentId { get; set; }

        [Required]
        public string Major { get; set; }

        [Required]
        public int GraduationYear { get; set; }
    }
}