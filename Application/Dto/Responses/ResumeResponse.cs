using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class ResumeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TopicResponse> Topics { get; set; }
    }
}