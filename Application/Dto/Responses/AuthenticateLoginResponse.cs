namespace Application.Dto.Responses
{
    public class AuthenticateLoginResponse
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public string TokenStr { get; set; }
        public string ImageToken { get; set; }
    }
}