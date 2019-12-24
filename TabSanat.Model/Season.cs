using System;
using System.Collections.Generic;
using TabSanat.Model.Base;

namespace TabSanat.Model
{
    public class Season: BaseEntity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Course> Courses { get; set; }
        public int MonthsInSeason
        {
            get
            {
                var yearDifference = EndDate.Year - StartDate.Year;
                var monthDifference = EndDate.Month - StartDate.Month;

                return (yearDifference * 12) + (monthDifference) + 1;
            }
        }
        public bool SeasonEnded
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
