using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Responses
{
    public class AuthenticateLoginResponse
    {
        public bool Succeeded { get; set; }
        public string TokenStr { get; set; }
    }
}
