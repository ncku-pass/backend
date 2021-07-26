using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Responses
{
    public class NCKUPortalTokenVerifyResponse
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
        public string StudentId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
        public int EnrollmentYear { get; set; }
    }
}
