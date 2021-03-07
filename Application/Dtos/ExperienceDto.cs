using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public class ExperienceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public ICollection<TagDto> Tags { get; set; }
    }
}
