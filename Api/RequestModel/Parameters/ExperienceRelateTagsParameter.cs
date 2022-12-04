namespace Api.RequestModel.Parameters
{
    public class ExperienceRelateTagsParameter
    {
        public int ExperienceId { get; set; }
        public int[] TagIds { get; set; }
    }
}