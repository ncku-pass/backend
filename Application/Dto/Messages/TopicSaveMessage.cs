using System.Collections.Generic;

namespace Application.Dto.Messages
{
    public class TopicSaveMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ExperienceId { get; set; }
    }
}