
using System.Collections.Generic;

namespace IMP.Models
{ 

    public class Section
    {

        public string Id { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }


        public string StartTime { get; set; }

        public string Duration { get; set; }

        public List<string> SelectedDays { get; set; }
    }
}
