﻿using System.Collections.Generic;

namespace Application.Dto.Messages
{
    public class ResumeSaveMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CardSaveMessage> Cards { get; set; }
        public List<int> DeleteCardIds { get; set; }

        public void InitOrder()
        {
            var i = 1;
            foreach (var card in this.Cards)
            {
                card.Order = i;
                i++;
                var j = 1;
                foreach (var exp in card.Experiences)
                {
                    exp.Order = j;
                    j++;
                }
            }
        }
    }

    public class CardSaveMessage
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
        public List<ExpInCardMessage> Experiences { get; set; }
        public List<int> DeleteExpIds { get; set; }
    }

    public class ExpInCardMessage
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}