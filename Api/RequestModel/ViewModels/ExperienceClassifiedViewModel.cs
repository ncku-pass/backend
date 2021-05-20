using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class ExperienceClassifiedViewModel
    {
        public List<ExperienceViewModel> Course { get; set; }
        public List<ExperienceViewModel> Activity { get; set; }
        public List<ExperienceViewModel> Competition { get; set; }
        public List<ExperienceViewModel> Work { get; set; }
        public List<ExperienceViewModel> Certificate { get; set; }
        public List<ExperienceViewModel> Other { get; set; }
    }
}