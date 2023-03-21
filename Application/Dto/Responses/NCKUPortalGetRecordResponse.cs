using System.Collections.Generic;

namespace Application.Dto.Responses
{
    public class Activity
    {
        private string _name;

        public string Activity_name
        {
            get => _name;
            set => _name = value.Length > 100 ? value.Substring(0, 97) + "..." : value;
        }
        public string Activity_url { get; set; }
        public string Active_start { get; set; }
        public string Active_end { get; set; }
        public int Active_start_time { get; set; }
        public int Active_end_time { get; set; }
    }

    public class Club
    {
        private string _name;

        public string Club_name
        {
            get => _name;
            set => _name = value.Length > 100 ? value.Substring(0, 97) + "..." : value;
        }
        public string Syear { get; set; }
        public string Sem { get; set; }
        public string Position { get; set; }
    }

    public class Course
    {
        private string _name;

        public string Course_name
        {
            get => _name;
            set => _name = value.Length > 100 ? value.Substring(0, 97) + "..." : value;
        }
        public string Syear { get; set; }
        public string Sem { get; set; }
        public string Dept_code { get; set; }
        public string Serial { get; set; }
        public string Course_code { get; set; }
        public string Course_url { get; set; }
        public string Course_ename { get; set; }
        public string S_m { get; set; }
        public object Core_abilities { get; set; }
    }

    public class Data
    {
        public List<Course> Course { get; set; }
        public List<Club> Club { get; set; }
        public List<Activity> Activity { get; set; }
    }

    public class NCKUPortalGetRecordResponse
    {
        public bool Status { get; set; }
        public string Msg { get; set; }
        public string Type { get; set; }
        public string Student_id { get; set; }
        public Data Data { get; set; }
    }
}

