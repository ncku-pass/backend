using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.Domains
{
    public class Tag_ExperienceDomain
    {
        public int Id { get; set; }
        public int ExperienceId { get; set; }
        public int TagId { get; set; }
    }
}
