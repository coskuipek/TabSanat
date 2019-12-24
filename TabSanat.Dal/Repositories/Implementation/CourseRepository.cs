using System;
using System.Collections.Generic;
using System.Text;
using TabSanat.Dal.Data;
using TabSanat.Dal.Repositories.Implementation.Base;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;

namespace TabSanat.Dal.Repositories.Implementation
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(TabDbContext context) : base(context)
        {

        }
    }
}
