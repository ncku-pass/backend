using Infrastructure.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Experience : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(10)]
        public string Position { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string CoreAbilities { get; set; }


        [MaxLength(250)]
        public string Feedback { get; set; }

        [Required]
        [MaxLength(5)]
        public string Semester { get; set; }

        [Url]
        [Column(TypeName = "Text")]
        public string Link { get; set; }

        public ExperienceType ExperienceType { get; set; }

        [Required]
        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateEnd { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}