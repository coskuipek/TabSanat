using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Model;

namespace TabSanat.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IQueryable<Course>> GetAllAsync(Expression<Func<Course, bool>> filter = null, Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = null, params Expression<Func<Course, object>>[] includes);

        decimal CourseTotalPrice(Guid courseId);
        void AddCourseToDatabase(Course course);
        void DeleteCourse(Guid courseId);
        Task<Course> GetCourseAsync(Expression<Func<Course, bool>> predicate, params Expression<Func<Course, object>>[] includes);
        List<DateTime> CourseLessonDates(Course course);
        List<DateTime> CourseLessonDates(DateTime startDate, DateTime endDate, DayOfWeek dayOfLessons);
        int CourseLessonCount(DateTime startDate, DateTime endDate, DayOfWeek dayOfLessons);
        int NumberOfLessonsForStudent(Course course, DateTime registerDate);
        int NumberOfUnpaidLessons(Registration registration);
        decimal PriceOfUnpaidLessons(Registration registration);
        decimal ExpectedPaymentUntilNow(Registration registration);

    }
}
