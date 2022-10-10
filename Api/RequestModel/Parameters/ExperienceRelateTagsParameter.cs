using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.Parameters
{
    public class ExperienceRelateTagsParameter
    {
        public int ExperienceId { get; set; }
        public int[] TagIds { get; set; }

    }
}
