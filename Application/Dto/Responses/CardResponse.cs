using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class CardResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int ResumeId { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
        public ICollection<ExpInCardResponse> Experiences { get; set; }
    }
}