using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.ViewModels
{
    public class ExperienceClassifiedViewModel
    {
        public List<ExperienceViewModel> Courses { get; set; }
        public List<ExperienceViewModel> Activities { get; set; }
        public List<ExperienceViewModel> Competitions { get; set; }
        public List<ExperienceViewModel> Works { get; set; }
        public List<ExperienceViewModel> Certificates { get; set; }
        public List<ExperienceViewModel> Others { get; set; }
    }
}
