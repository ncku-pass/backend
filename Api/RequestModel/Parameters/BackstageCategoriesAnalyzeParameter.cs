using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.Parameters
{
    public class BackstageCategoriesAnalyzeParameter
    {
        public BackstageCategoriesAnalyzeCollegesParameter Colleges { get; set; }
        public List<string> Departments { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public List<string> Categories { get; set; }
    }

    public class BackstageCategoriesAnalyzeCollegesParameter
    {
        public List<string> Bachelor { get; set; }
        public List<string> Master { get; set; }
    }
}
