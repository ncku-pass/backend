using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.Parameters
{
    public class TopicSaveParameter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> ExperienceId { get; set; }
    }
}
