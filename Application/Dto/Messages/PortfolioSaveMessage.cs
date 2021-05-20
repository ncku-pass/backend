﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class PortfolioSaveMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TopicSaveMessage> Topics { get; set; }
    }
}
