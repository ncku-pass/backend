using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public class ResumeSaveParameter
    {
        public string Name { get; set; }
        public List<TopicSaveParameter> Topics { get; set; }
    }
}