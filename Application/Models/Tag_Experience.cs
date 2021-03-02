using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.Models
{
    public class Tag_Experience
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ExperienceId")]
        public int E_Id { get; set; }
        public Experience Experience { get; set; }

        [ForeignKey("TagId")]
        public int T_Id { get; set; }
        public Tag Tag { get; set; }
    }
}
