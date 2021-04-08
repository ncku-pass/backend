using System.ComponentModel.DataAnnotations;

namespace Api.RequestModel.Parameters
{
    public class AuthenticateLoginParameter
    {
        [Required]
        public string StudentId { get; set; }

        [Required]
        public string Password { get; set; }
    }
}