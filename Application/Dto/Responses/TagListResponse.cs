using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Responses
{
    public class TagListResponse
    {
        public int? ExperienceId { get; set; }
        public ICollection<TagResponse> Tags { get; set; }
    }
}
