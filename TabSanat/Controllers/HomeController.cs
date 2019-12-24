using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;

namespace TabSanat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IRegisterService _registerService;
        private readonly IAppSettingsService _appSettings;

        public HomeController(IStudentService studentService, ICourseService courseService, IRegisterService registerService, IAppSettingsService appSettings)
        {
            _studentService = studentService;
            _courseService = courseService;
            _registerService = registerService;
            _appSettings = appSettings;
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Test()
        {
            return View();
        }
        public IActionResult Test2()
        {
            throw new Exception("Test Excetion");

        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            #region Birthday Calculator
            var allStudents = await _studentService.GetAllAsync(x => x.BirthDate != null, x => x.OrderByDescending(y => y.BirthDate.Value.Month).ThenByDescending(z => z.BirthDate.Value.Day));


            foreach (var student in allStudents)
            {
                var birthday = student.BirthDate.Value;

                int years = DateTime.Now.Year - birthday.Year;
                birthday = birthday.AddYears(years);
                DateTime checkWeek = DateTime.Now.Date.AddDays(8);
                DateTime checkMonth = DateTime.Now.Date.AddDays(31);

                if (birthday == DateTime.Now.Date)
                {
                    var studentModel = new StudentViewModel()
                    {
                        FullName = student.FullName,
                        BirthDate = student.BirthDate
                    };
                    model.BirthdatesToday.Add(studentModel);
                }
                if ((birthday > DateTime.Now.Date) && (birthday < checkWeek))
                {
                    var studentModel = new StudentViewModel()
                    {
                        FullName = student.FullName,
                        BirthDate = student.BirthDate
                    };
                    model.BirthdatesThisWeek.Add(studentModel);
                }
                if ((birthday < checkMonth) && (birthday >= checkWeek))
                {
                    var studentModel = new StudentViewModel()
                    {
                        FullName = student.FullName,
                        BirthDate = student.BirthDate
                    };
                    model.BirthdatesThisMonth.Add(studentModel);
                }
            }
            #endregion

            model.CoursesList = new List<CourseViewModel>();

            var courses = await _courseService.GetAllAsync(x => x.CoursesEnded == false);

            foreach (var course in courses)
            {
                var courseModel = new CourseViewModel()
                {
                    Id = course.Id,
                    Name = course.Name
                };

                var registers = await _registerService.GetAllAsync(x => x.Course == course, null,
                                                                    x => x.Student, x => x.Course);

                courseModel.Registers = new List<RegisterViewModel>();
                foreach (var register in registers)
                {
                    var registerModel = new RegisterViewModel()
                    {
                        StudentId = register.StudentId,
                        StudentName = register.Student.FullName,
                        NumberOfLatePayments = _courseService.NumberOfUnpaidLessons(register)
                    };

                    var numberToMarkLate = await _appSettings.GetSettingAsync(
                                    x => x.Id == 2);
                    if (registerModel.NumberOfLatePayments > Convert.ToInt32(numberToMarkLate.Value))
                        courseModel.Registers.Add(registerModel);

                }

                model.CoursesList.Add(courseModel);
            }


            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
