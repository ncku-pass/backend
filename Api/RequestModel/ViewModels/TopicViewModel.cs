using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class TopicViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ExperienceViewModel> Experiences { get; set; }
    }
}
