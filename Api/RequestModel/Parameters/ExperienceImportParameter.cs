using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.Parameters
{
    public class ExperienceImportParameter
    {
        public string Name { get; set; }
        public string Position { get; set; } // 只有社團有
        public string Semester { get; set; } // 只有課程有
        public string Link { get; set; } // 只有課程有
        public string ExperienceType { get; set; }
        public DateTime DateStart { get; set; } // 只有社團、活動有
        public DateTime? DateEnd { get; set; } // 只有社團、活動有
    }
}
