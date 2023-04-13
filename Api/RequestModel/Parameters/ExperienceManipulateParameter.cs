using System;
using System.Collections.Generic;

namespace Api.RequestModel.Parameters
{
    public abstract class ExperienceManipulateParameter
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => _name = value.Length > 100 ? value.Substring(0, 100) : value;
        }

        public string Position { get; set; }
        public string Description { get; set; }
        public string Feedback { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public string[] Categories { get; set; }
        public string Type { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int[] Tags { get; set; }
        public List<ImgInExpParameter> Images { get; set; }
    }

    public class ImgInExpParameter { 
        public int Id { get; set; }
        public string Name { get; set; }
    }
}