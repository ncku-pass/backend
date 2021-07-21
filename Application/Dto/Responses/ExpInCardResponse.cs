using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Responses
{
    public class ExpInCardResponse : ExperienceResponse
    {
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}
