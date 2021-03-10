using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class TagListResponse
    {
        public int? ExperienceId { get; set; }
        public ICollection<TagResponse> Tags { get; set; }
    }
}