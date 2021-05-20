using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class ResumeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TopicViewModel> Topics { get; set; }
    }
}