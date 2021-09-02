using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class User : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(9)]
        public string StudentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public int DepartmentId { get; set; }

        [Required]
        public int EnrollmentYear { get; set; }

        [Required]
        public int GraduationYear { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        //[Required]
        public Guid AspNetId { get; set; }
    }
}