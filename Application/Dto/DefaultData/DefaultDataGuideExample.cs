using Application.Dto.Messages;
using System.Collections.Generic;

namespace Application.Dto.Data
{
    class DefaultDataGuideExample
    {
        public string[] TagCreateMessages { get; set; }
        public List<ExperienceCreateMessage> ExperienceCreateMessages { get; set; }
        public ResumeSaveMessage ResumeSaveMessage { get; set; }
    }
}
