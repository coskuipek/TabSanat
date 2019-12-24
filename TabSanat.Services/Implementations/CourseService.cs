using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TabSanat.Dal.Repositories.Interfaces;
using TabSanat.Model;
using TabSanat.Services.Interfaces;

namespace TabSanat.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IRegistrationRepository _registrationRepository;

        public CourseService(ICourseRepository courseRepository, IRegistrationRepository registrationRepository)
        {
            _courseRepository = courseRepository;
            _registrationRepository = registrationRepository;
        }


        public void AddCourseToDatabase(Course course)
        {
            _courseRepository.Add(course);
        }

        public List<DateTime> CourseLessonDates(Course course)
        {
            var list = new List<DateTime>();

            for (var dt = course.StartDate.Date; dt.Date <= course.EndDate.Date; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == course.DayOfWeek)
                {
                    list.Add(dt);
                }
            }
            return list;
        }

        public List<DateTime> CourseLessonDates(DateTime startDate, DateTime endDate, DayOfWeek dayOfLessons)
        {
            var list = new List<DateTime>();

            for (var dt = startDate.Date; dt.Date <= endDate.Date; dt = dt.AddDays(1))
            {
                if (dt.DayOfWeek == dayOfLessons)
                {
                    list.Add(dt);
                }
            }
            return list;
        }

        public int CourseLessonCount(DateTime startDate, DateTime endDate, DayOfWeek dayOfLessons)
        {
            var list = CourseLessonDates(startDate, endDate, dayOfLessons);

            return list.Count;
        }

        public decimal CourseTotalPrice(Guid courseId)
        {
            decimal totalPrice = 0;
            var all =  _registrationRepository.GetMany(x => x.CourseId == courseId);
            foreach (var item in all)
                totalPrice += item.Price;
            return totalPrice;
        }

        public void DeleteCourse(Guid courseId)
        {
            _courseRepository.Remove(courseId);
        }


        public async Task<IQueryable<Course>> GetAllAsync(Expression<Func<Course, bool>> filter = null, Func<IQueryable<Course>, IOrderedQueryable<Course>> orderBy = null, params Expression<Func<Course, object>>[] includes)
        {
            return await _courseRepository.GetAllAsync(filter, orderBy, includes);
        }

        public async Task<Course> GetCourseAsync(Expression<Func<Course, bool>> predicate, params Expression<Func<Course, object>>[] includes)
        {
            return await _courseRepository.GetFirstOrDefaultAsync(predicate, includes);
        }

        public int NumberOfLessonsForStudent(Course course, DateTime registerDate)
        {
            var lessons = new List<DateTime>();

            if (course.StartDate.Date > registerDate.Date)
            {
               lessons = CourseLessonDates(course.StartDate, course.EndDate, course.DayOfWeek);
            }
            else
            {
               lessons = CourseLessonDates(registerDate, course.EndDate, course.DayOfWeek);
            }
            

            return lessons.Count();
        }

        public int NumberOfUnpaidLessons(Registration registration)
        {
            var lessons = CourseLessonDates(registration.StartToCourseDate, registration.Course.EndDate,registration.Course.DayOfWeek);
            
            var studentPricePerLesson = registration.Price;
            var studentTotalRegisterPrice = studentPricePerLesson * registration.NrOfLessonStudentWillJoin;
            int numberOfLatePayments = 0;

            var studentRegisterDate = registration.StartToCourseDate;

            var lateDates = lessons.Where(x => x.Date <= DateTime.Now.Date && studentRegisterDate <= x.Date);

            numberOfLatePayments = lateDates.Count();

            var paidForNow = studentTotalRegisterPrice - registration.PaymentLeft;
            if (paidForNow == 0)
            {
                return numberOfLatePayments;
            }
            if (paidForNow < 0)
            {
                return lateDates.Count();
            }

            numberOfLatePayments -= (int)(paidForNow / studentPricePerLesson);

            if (numberOfLatePayments < 0)
            {
                return 0;
            }
            return numberOfLatePayments;
        }

        public decimal ExpectedPaymentUntilNow(Registration registration)
        {

            var singlePayment = registration.Price;

            var lessons = CourseLessonDates(registration.Course);
            var studentRegisterDate = registration.StartToCourseDate;

            var lateDates = lessons.Where(x => x.Date < DateTime.Now && studentRegisterDate <= x.Date);

            return lateDates.Count() * singlePayment;
        }

        public decimal PriceOfUnpaidLessons(Registration registration)
        {
            var studentRegisterPrice = registration.Price;
            var numberOfLatePayments = NumberOfUnpaidLessons(registration);

            return numberOfLatePayments * studentRegisterPrice;
        }

    }
}
