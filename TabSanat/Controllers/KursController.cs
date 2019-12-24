using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Helpers;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class KursController : Controller
    {

        private readonly ICourseService _courseService;
        private readonly IStudentService _studentService;
        private readonly ISeasonService _seasonService;
        private readonly IGroupService _groupService;
        private readonly IRegisterService _registerService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public KursController(ICourseService courseService, IStudentService studentService, ISeasonService seasonService, IGroupService groupService, IRegisterService registerService, UserManager<AppUser> userManager, ISaveService save)
        {
            _courseService = courseService;
            _studentService = studentService;
            _seasonService = seasonService;
            _groupService = groupService;
            _registerService = registerService;
            _userManager = userManager;
            _save = save;
        }

        [Authorize(Policy = "Kurs Liste")]
        public async Task<IActionResult> Index()
        {
            var courses = await _courseService.GetAllAsync(null,null,
                                                                    x => x.Season, y => y.Registrations, x=> x.Groups);

            List<CourseViewModel> listModel = new List<CourseViewModel>();

            foreach (var course in courses)
            {
                var model = new CourseViewModel
                {
                    Id = course.Id,
                    Name = $"{course.Name} {Translator.DayName(course.DayOfWeek)}",
                    OneLessonPrice = course.OneLessonPrice,
                    CourseEnded = course.CoursesEnded,
                    SeasonName = course.Season.Name,
                    CountOfStudents = course.Registrations.Count(x => x.LeaveDate == null),
                    Groups = string.Join(",",course.Groups.Select(x=> x.Name))
                };
                listModel.Add(model);
            }
            return View(listModel);
        }

        [Authorize(Policy = "Kurs Detay")]
        public async Task<IActionResult> Details(Guid? id, Guid? groupid)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var course = await _courseService.GetCourseAsync(x => x.Id == id, y => y.Season, z => z.Registrations, x=> x.Groups);
            if (course == null)
                return NotFound();

            CourseViewModel model = new CourseViewModel()
            {
                Id = course.Id,
                Name = course.Name,
                OneLessonPrice = course.OneLessonPrice,
                SeasonName = course.Season.Name,
                DayOfWeekName = new System.Globalization.CultureInfo("tr-TR").DateTimeFormat.DayNames[(int)course.DayOfWeek],
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                TotalNumberOfLessons = course.TotalNumberOfLessons,
                CountOfStudents = course.Registrations.Count(x => x.LeaveDate == null),
                CourseEnded = course.CoursesEnded
            };

            model.LessonDates = _courseService.CourseLessonDates(course);

            IQueryable<Registration> registers = Enumerable.Empty<Registration>().AsQueryable();
            if (groupid == null ||groupid == Guid.Empty)
            {
                registers = await _registerService.GetAllAsync(x => x.Course == course, null,
                                                                x => x.Student, x => x.Course, x => x.Group);
                ViewData["GroupId"] = new SelectList(course.Groups, "Id", "Name");
            }
            else
            {
                registers = await _registerService.GetAllAsync(x => x.Course == course && x.GroupId == groupid, null, x => x.Student, x => x.Course, x => x.Group);
                ViewData["GroupId"] = new SelectList(course.Groups, "Id", "Name", groupid);
            }

            
            model.Registers = new List<RegisterViewModel>();
            foreach (var register in registers)
            {
                RegisterViewModel registerView = new RegisterViewModel()
                {
                    Id = register.Id,
                    StudentName = register.Student.FullName,
                    StudentId = register.StudentId,
                    StartToCourseDate = register.StartToCourseDate,
                    Price = register.Price,
                    NrOfLessonStudentWillJoin = register.NrOfLessonStudentWillJoin,
                    NumberOfLatePayments = _courseService.NumberOfUnpaidLessons(register),
                    LeaveDate = register.LeaveDate,
                    PaymentLeft = register.PaymentLeft,
                    GroupName = register.Group == null ? "-" : register.Group.Name
                };
                model.Registers.Add(registerView);
            }


            

            return View(model);
        }

        [Authorize(Policy = "Kurs Ekle")]
        public async Task<IActionResult> Create(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var season = await _seasonService.GetSeasonAsync(x => x.Id == id);
            var model = new CourseFormModel
            {
                StartDate = season.StartDate,
                EndDate = season.EndDate,
                SeasonId = season.Id
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult LessonCount(DateTime startdate, DateTime enddate, int dayofweek, decimal oneprice)
        {
            var list = new List<string>();
            var lessonCount = _courseService.CourseLessonCount(startdate, enddate, (DayOfWeek)dayofweek);

            list.Add(lessonCount.ToString());
            if (oneprice > 0)
            {
                list.Add((oneprice * lessonCount).ToString());
            }


            var result = new { data = list };

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Course course = new Course()
                {
                    Name = _save.FixName(model.Name),
                    OneLessonPrice = model.OneLessonPrice,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    SeasonId = model.SeasonId,
                    DayOfWeek = model.DayOfWeek.Value,
                    AppUserId = user.Id
                };
                course.TotalNumberOfLessons = _courseService.CourseLessonCount(model.StartDate, model.EndDate, model.DayOfWeek.Value);
                course.Groups = new List<Group>();
                foreach (var group in model.Groups)
                {
                    var newGroup = new Group()
                    {
                        Name = group,
                        Course = course
                    };
                    course.Groups.Add(newGroup);
                }


                _courseService.AddCourseToDatabase(course);
                var changes = await _save.Completeasync("Yeni kurs oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni kurs oluşturuldu";

                if (model.ContinueAdding == true)
                    return RedirectToAction("Create", new { id = model.SeasonId });

                return RedirectToAction(nameof(Index));
            }
            ViewData["SeasonId"] = new SelectList(await _seasonService.GetAllAsync(), "Id", "Name", model.SeasonId);
            return View(model);
        }


        [Authorize(Policy = "Kurs Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var course = await _courseService.GetCourseAsync(x => x.Id == id);
            if (course == null)
                return NotFound();


            CourseFormModel model = new CourseFormModel()
            {
                Id = course.Id,
                Name = course.Name,
                DayOfWeek = course.DayOfWeek,
                StartDate = course.StartDate,
                EndDate = course.EndDate,
                OneLessonPrice = course.OneLessonPrice,
                SeasonId = course.SeasonId
            };

            ViewData["SeasonId"] = new SelectList(await _seasonService.GetAllAsync(), "Id", "Name", course.SeasonId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CourseFormModel model)
        {
            if (id != model.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var course = await _courseService.GetCourseAsync(x => x.Id == id);
                course.Name = _save.FixName(model.Name);
                course.OneLessonPrice = model.OneLessonPrice;
                course.DayOfWeek = model.DayOfWeek.Value;

                var changes = await _save.Completeasync("Kurs düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Kurs düzenlendi";


                return RedirectToAction(nameof(Index));
            }
            ViewData["SeasonId"] = new SelectList(await _seasonService.GetAllAsync(), "Id", "Name", model.SeasonId);
            return View(model);
        }

        public async Task<IActionResult> Gruplar(Guid? courseid)
        {
            var courses = await _courseService.GetAllAsync(x => x.CoursesEnded == false,null, x=> x.Groups);
            ViewData["CourseId"] = CourseSelects.CourseNameDayList(courses);

            if (courseid == null)
            {
                var model = new GroupSaveFormModel()
                {
                    GroupItems = new List<GroupItem>()
                };
                

                return View(model);
            }
            else
            {
                var course = courses.FirstOrDefault(x => x.Id == courseid);

                var model = new GroupSaveFormModel()
                {
                    CourseId = courseid.Value
                };
                model.GroupItems = new List<GroupItem>();
                foreach (var group in course.Groups)
                {
                    var modelItem = new GroupItem()
                    {
                        Id =group.Id,
                        Name = group.Name
                    };
                    model.GroupItems.Add(modelItem);
                }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Gruplar(GroupSaveFormModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var course = await _courseService.GetCourseAsync(x => x.Id == model.CourseId, x => x.Groups);
            if (course.Groups.Count > 0)
            {
                foreach (var item in model.GroupItems)
                {
                    var group = await _groupService.GetGroupAsync(x => x.Id == item.Id);

                    if (item.DeleteThis == true)
                    {
                        _groupService.DeleteGroup(group.Id);
                    }
                    else
                    {
                        group.Name = item.Name;
                    }
                }
            }

            if (!string.IsNullOrEmpty(model.NewName))
            {
                var group = new Group()
                {
                    Name = model.NewName,
                    Course = course
                };
                if (course.Groups == null)
                {
                    course.Groups = new List<Group>();
                }
                course.Groups.Add(group);
            }

            var changes = await _save.Completeasync("Grup adları düzenlendi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Grup adları düzenlendi";

            ViewData["CourseId"] = CourseSelects.CourseNameDayList(await _courseService.GetAllAsync());
            return RedirectToAction("Gruplar",new { courseid = course.Id }) ;
        }

        public async Task<IActionResult> Group(Guid? id)
        {
            var course = await _courseService.GetCourseAsync(x => x.Id == id, x => x.Groups);
            var model = new GroupSaveFormModel();
            model.CourseId = course.Id;
            model.GroupItems = new List<GroupItem>();
            foreach (var group in course.Groups)
            {
                var modelItem = new GroupItem()
                {
                    Id = group.Id,
                    Name = group.Name
                };
                model.GroupItems.Add(modelItem);
            }

            return PartialView("_GrupListe", model);
        }

        [Authorize(Policy = "Kurs Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var course = await _courseService.GetCourseAsync(x => x.Id == id, y => y.Season);
            if (course == null)
                return NotFound();

            var model = new CourseViewModel()
            {
                Id = course.Id,
                Name = course.Name,
                OneLessonPrice = course.OneLessonPrice,
                SeasonName = course.Season.Name
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _courseService.GetCourseAsync(x => x.Id == id, y => y.Season);

            _courseService.DeleteCourse(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Kurs silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Kurs silindi";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> GetDatesOfCourse(Guid registerid)
        {
            var register = await _registerService.GetRegistrationAsync(x => x.Id == registerid,
                x => x.Course, x => x.Payments);

            var allDates = _courseService.CourseLessonDates(register.StartToCourseDate, register.Course.EndDate, register.Course.DayOfWeek)
                .OrderByDescending(x => x.Date)
                .Select(x => new SelectListItem
                {
                    Value = x.ToShortDateString(),
                    Text = x.ToShortDateString()
                }).ToList();
            if (register.Payments.Count == 0)
            {
                allDates.Add(new SelectListItem { Text = "Ödeme alınmasın", Value = "12.12.3000" });
            }

            var result = new { data = allDates };
            return Json(result);
        }
        public async Task<IActionResult> CheckStartDate(DateTime startdate, Guid seasonid)
        {
            var season = await _seasonService.GetSeasonAsync(x => x.Id == seasonid);

            if (season.StartDate > startdate || startdate > season.EndDate)
                return Json(data: $"Kurs başlangıç tarihi sezona uymuyor.");

            else
                return Json(data: true);
        }

        public async Task<IActionResult> CheckEndDate(DateTime enddate, Guid seasonid)
        {
            var season = await _seasonService.GetSeasonAsync(x => x.Id == seasonid);

            if (season.StartDate > enddate || enddate > season.EndDate)
                return Json(data: $"Kurs bitiş tarihi sezona uymuyor.");

            else
                return Json(data: true);
        }

    }
}
