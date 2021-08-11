using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public class ResumeSaveParameter
    {
        public string Name { get; set; }
        public List<CardSaveParameter> Cards { get; set; }
        public List<int> DeleteCardIds { get; set; }
    }

    public class CardSaveParameter
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<ExpInCardParameter> Experiences { get; set; }
        public List<int> DeleteExpIds { get; set; }
    }

    public class ExpInCardParameter
    {
        public int Id { get; set; }
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}