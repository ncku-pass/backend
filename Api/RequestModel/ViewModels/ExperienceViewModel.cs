using System;
using System.Collections.Generic;

namespace Api.RequestModel.ViewModels
{
    public class ExperienceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Description { get; set; }
        public string CoreAbilities { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public string[] Categories { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public ICollection<TagViewModel> Tags { get; set; }
        public ICollection<ImageViewModel> Images { get; set; }
    }
}