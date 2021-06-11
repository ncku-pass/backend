using System;
using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class ExperienceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public string ExperienceType { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public ICollection<TagResponse> Tags { get; set; }
    }
}