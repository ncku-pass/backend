using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class AuthenticateLoginMessage
    {
        public string StudentId { get; set; }
        public string Password { get; set; }

    }
}
