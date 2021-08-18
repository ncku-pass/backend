namespace Application.Dto.Responses
{
    public class ExpInCardResponse : ExperienceResponse
    {
        public int Order { get; set; }
        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}