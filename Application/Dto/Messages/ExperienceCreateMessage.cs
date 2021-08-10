namespace Application.Dto.Messages
{
    public class ExperienceCreateMessage : ExperienceManipulateMessage
    {
        public string CoreAbilities { get; set; } // 只有ExperienceImportParameter
    }
}