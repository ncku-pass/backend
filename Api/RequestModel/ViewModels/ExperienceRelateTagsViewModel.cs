using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.ViewModels
{
    public class ExperienceRelateTagsViewModel
    {
        public int ExperienceId { get; set; }
        public int[] TagIds { get; set; }
    }
}
