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
        public string ExperienceType { get; set; }
    }
}