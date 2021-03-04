using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Infrastructure.Models
{
    public class Tag_Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ExperienceId")]
        public virtual Experience Experience { get; set; }
        public int ExperienceId { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
