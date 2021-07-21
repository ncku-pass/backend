using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public class ResumeSaveParameter
    {
        public string Name { get; set; }
        public List<CardSaveParameter> Cards { get; set; }
        public List<DeleteCardParameter> DeleteCards { get; set; }
    }

    public class CardSaveParameter
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ExpInCardParameter> Experiences { get; set; }
        public List<int> DeleteExpIds { get; set; }
    }

    public class ExpInCardParameter
    {
        public int Id { get; set; }
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }

    public class DeleteCardParameter
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}