namespace Application.Dto.Messages
{
    public class AuthenticateRegisterMessage
    {
        public bool Succeeded { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string StudentId { get; set; }
        public string Major { get; set; }
        public int GraduationYear { get; set; }
    }
}