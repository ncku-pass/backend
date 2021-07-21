using Infrastructure.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Card : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("ResumeId")]
        public virtual Resume Resume { get; set; }

        public int ResumeId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int UserId { get; set; }

        [Required]
        public CardType CardType { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public int Order { get; set; }
    }
}