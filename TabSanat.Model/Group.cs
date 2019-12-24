using System;
using System.Collections.Generic;
using System.Text;

namespace TabSanat.Model
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
