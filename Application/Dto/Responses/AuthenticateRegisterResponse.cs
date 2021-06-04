using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class AuthenticateRegisterResponse
    {
        public bool Succeeded { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}