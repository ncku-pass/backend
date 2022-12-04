using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public class BackstageCategoriesAnalyzeParameter
    {
        public BackstageCategoriesAnalyzeParameter()
        {
            YearStart ??= 1;
            YearEnd ??= 200;
        }

        public BackstageCategoriesAnalyzeCollegesParameter Colleges { get; set; }
        public List<string> Departments { get; set; }
        public int? YearStart { get; set; }
        public int? YearEnd { get; set; }
        public List<string> Categories { get; set; }
    }

    public class BackstageCategoriesAnalyzeCollegesParameter
    {
        public List<string> Bachelor { get; set; }
        public List<string> Master { get; set; }
    }
}