using Infrastructure.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto.Messages
{
    public class ExperienceUpdateMessage : ExperienceManipulateMessage
    {
        public int Id { get; set; }
        public int[] AddTags { get; set; }
        public int[] DropTags { get; set; }
    }
}
