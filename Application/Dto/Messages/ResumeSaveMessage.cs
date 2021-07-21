using System.Collections.Generic;

namespace Application.Dto.Messages
{
    public class ResumeSaveMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CardSaveMessage> Cards { get; set; }
        public List<DeleteCardMessage> DeleteCards { get; set; }
        public void InitCardOrder()
        {
            var i = 1;
            foreach (var item in this.Cards)
            {
                item.Order = i;
                i++;
            }
        }
    }

    public class CardSaveMessage
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public List<ExpInCardMessage> Experiences { get; set; }
        public List<int> DeleteExpIds { get; set; }
    }

    public class ExpInCardMessage
    {
        public int Id { get; set; }
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }

    public class DeleteCardMessage
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

}