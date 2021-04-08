namespace Application.Dto.Messages
{
    public class ExperienceCreateMessage : ExperienceManipulateMessage
    {
        public int[] AddTags { get; set; }
    }
}