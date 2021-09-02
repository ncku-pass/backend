using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class BackstageCategoriesAnalyzeMessage
    {
        public List<string> Degrees { get; set; }
        public List<string> Colleges { get; set; }
        public List<string> Departments { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public List<string> Categories { get; set; }
    }
}
