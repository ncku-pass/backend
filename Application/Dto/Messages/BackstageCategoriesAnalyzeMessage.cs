using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class BackstageCategoriesAnalyzeMessage
    {
        public BackstageCategoriesAnalyzeCollegesMessage Colleges { get; set; }
        public List<string> Departments { get; set; }
        public int YearStart { get; set; }
        public int YearEnd { get; set; }
        public List<string> Categories { get; set; }
    }
    public class BackstageCategoriesAnalyzeCollegesMessage
    {
        public List<string> Bachelor { get; set; }
        public List<string> Master { get; set; }
    }
}
