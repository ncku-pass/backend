using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Card_Experience : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CardId")]
        public virtual Card Card { get; set; }

        public int CardId { get; set; }

        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }

        public int ExperienceId { get; set; }

        public bool ShowPosition { get; set; }
        public bool ShowFeedback { get; set; }
    }
}