using System;

namespace Api.RequestModel.Parameters
{
    public abstract class ExperienceManipulateParameter
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public string[] Categories { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int[] Tags { get; set; }
    }
}