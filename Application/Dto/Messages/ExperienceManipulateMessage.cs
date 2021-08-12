using System;

namespace Application.Dto.Messages
{
    public abstract class ExperienceManipulateMessage
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public string Category { get; set; }
        public string ExperienceType { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int[] Tags { get; set; }
    }
}