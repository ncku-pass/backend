using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Topic_Experience : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }

        public int TopicId { get; set; }

        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }

        public int ExperienceId { get; set; }
    }
}