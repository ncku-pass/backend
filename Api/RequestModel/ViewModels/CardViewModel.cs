using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class CardViewModel
    {
        public string Type { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public List<ExpInCardViewModel> Experiences { get; set; }
    }
}