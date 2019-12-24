using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using TabSanat.Model;

namespace TabSanat.Helpers
{
    public static class CourseSelects
    {
        public static SelectList CourseNameDayList(IQueryable<Course> courses)
        {
            List<object> newList = new List<object>();

            foreach (var course in courses)
            {
                newList.Add(new
                {
                    Id = course.Id,
                    Name = $"{course.Name} {Translator.DayName(course.DayOfWeek)}"
                });
            }
            var selectList = new SelectList(newList, "Id", "Name");
            return selectList;
        }

       
        
    }
}
