using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class TopicResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ResumeId { get; set; }
        public ICollection<ExperienceResponse> Experiences { get; set; }
    }
}