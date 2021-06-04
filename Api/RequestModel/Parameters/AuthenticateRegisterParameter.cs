using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class AuthenticateRegisterParameter
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email格式錯誤")]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "密碼輸入不一致")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression(@"[a-zA-Z]\d{8}", ErrorMessage = "學號格式錯誤")]
        public string StudentId { get; set; }

        [Required]
        public string Major { get; set; }

        [Required]
        public int GraduationYear { get; set; }
    }
}