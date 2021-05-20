﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.RequestModel.Parameters
{
    public class PortfolioSaveParameter
    {
        public string Name { get; set; }
        public List<TopicSaveParameter> Topics { get; set; }
    }

}
