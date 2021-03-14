namespace Api.RequestModel.Parameters
{
    public class ExperienceUpdateParameter : ExperienceManipulateParameter
    {
        public int[] AddTags { get; set; }
        public int[] DropTags { get; set; }
    }
}
