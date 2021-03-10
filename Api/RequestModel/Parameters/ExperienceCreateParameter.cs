using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public class ExperienceCreateParameter
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public ICollection<TagCreateParameter> Tags { get; set; }
    }
}