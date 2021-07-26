using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class NCKUPortalRegisterMessage
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public string Major { get; set; }
        public int EnrollmentYear { get; set; }
    }
}
