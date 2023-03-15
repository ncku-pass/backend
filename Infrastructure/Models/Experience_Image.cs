using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Experience_Image : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }

        public int ExperienceId { get; set; }

        [ForeignKey("ImageId")]
        public virtual Image Tag { get; set; }

        public int ImageId { get; set; }
    }
}