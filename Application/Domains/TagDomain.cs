using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Application.Domains
{
    public class TagDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
