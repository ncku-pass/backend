using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class BackStageAbilityAnalyzeViewModel
    {
        public BackStageAbilityAnalyzeViewModel()
        {
            Tags = new List<BackStageAbilityAnalyzeTagViewModelItem>();
            Experiences = new List<BackStageAbilityAnalyzeExpViewModelItem>();
        }

        public int Headcount { get; set; }
        public List<BackStageAbilityAnalyzeTagViewModelItem> Tags { get; set; }
        public List<BackStageAbilityAnalyzeExpViewModelItem> Experiences { get; set; }
    }

    public class BackStageAbilityAnalyzeTagViewModelItem
    {
        public string Name { get; set; }
        public int Headcount { get; set; }
        public int Count { get; set; }
    }

    public class BackStageAbilityAnalyzeExpViewModelItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int Headcount { get; set; }
    }

}
