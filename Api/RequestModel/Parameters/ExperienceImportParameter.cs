﻿using System;

namespace Api.RequestModel.Parameters
{
    public class ExperienceImportParameter
    {
        public string Name { get; set; }
        public string Position { get; set; } // 只有社團有
        public string Semester { get; set; } // 只有課程有
        public string CoreAbilities { get; set; } // 只有課程有
        public string Link { get; set; } // 只有課程有
        public string[] Categories { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; } // 只有社團、活動有
        public DateTime? DateEnd { get; set; } // 只有社團、活動有
    }
}