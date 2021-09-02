﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Responses
{
    public class BackstageCategoriesAnalyzeResponse
    {
        public List<BackstageCategoriesAnalyzeResponseItem> Experiences { get; set; }
        public List<BackstageCategoriesAnalyzeResponseItem> Tags { get; set; }
    }

    public class BackstageCategoriesAnalyzeResponseItem
    {
        public string Name { get; set; }
        public int Count { get; set; }

    }
}