using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class BackStageAbilityAnalyzeResponse
    {
        public BackStageAbilityAnalyzeResponse()
        {
            Tags = new List<BackStageAbilityAnalyzeTagResponseItem>();
            Experiences = new List<BackStageAbilityAnalyzeExpResponseItem>();
        }

        public List<BackStageAbilityAnalyzeTagResponseItem> Tags { get; set; }
        public List<BackStageAbilityAnalyzeExpResponseItem> Experiences { get; set; }
    }

    public class BackStageAbilityAnalyzeTagResponseItem
    {
        public string Name { get; set; }
        public int HeadCount { get; set; }
        public int Count { get; set; }
    }

    public class BackStageAbilityAnalyzeExpResponseItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int HeadCount { get; set; }
    }
}
