using System;
using System.Collections.Generic;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Course: BaseEntity
    {
        public string Name { get; set; }
        public decimal OneLessonPrice { get; set; }
        public int TotalNumberOfLessons { get; set; }
        public Guid SeasonId { get; set; }
        public Season Season { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<Registration> Registrations { get; set; }
        public List<Group> Groups { get; set; }
        public bool CoursesEnded
        {
            get
            {
                if (DateTime.Now > EndDate)
                    return true;
                
                else
                    return false;
            }
        }
    }
}
