using Infrastructure.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class ExperienceUpdateMessage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public ExperienceType ExperienceType { get; set; }

        public int?[] AddTags { get; set; }
        public int?[] DropTags { get; set; }
    }
}
