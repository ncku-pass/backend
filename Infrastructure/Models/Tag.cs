﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Tag : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public int UserId { get; set; }
    }
}