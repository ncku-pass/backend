using System.Collections.Generic;

namespace Application.Dto.Messages
{
    public class ExperienceCreateMessage
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public ICollection<TagCreateMessage> Tags { get; set; }
    }
}