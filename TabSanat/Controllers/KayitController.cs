using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Helpers;
using TabSanat.Maps;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class KayitController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IStudentService _studentService;
        private readonly IExtraService _extraService;
        private readonly ICourseService _courseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IDiscountService _discountService;
        private readonly ISaveService _save;

        public KayitController(IRegisterService registerService, IStudentService studentService, IExtraService extraService, ICourseService courseService, UserManager<AppUser> userManager, IDiscountService discountService, ISaveService save)
        {
            _registerService = registerService;
            _studentService = studentService;
            _extraService = extraService;
            _courseService = courseService;
            _userManager = userManager;
            _discountService = discountService;
            _save = save;
        }
        [Authorize(Policy = "Kayit Liste")]
        public async Task<IActionResult> Index()
        {
            var registers = await _registerService.GetAllAsync(null, null,
                                                        x => x.Student, x => x.Course, 
                                                        x => x.Course.Season, x => x.Discount, x=> x.Group);

            var model = new RegisterMaps().RegisterIndexMap(registers);

            ViewData["Heading"] = "Tüm Kayıtlar";
            ViewData["Title"] = "Tüm Kayıtlar";
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarihFiltre(DateTime startdate, DateTime enddate)
        {
            var registers = await _registerService.GetAllAsync(null, null,
              y => y.Student, y => y.Course, y => y.Course.Season, y => y.Discount);

            if (startdate != DateTime.MinValue)
                registers = registers.Where(x => x.RegisterDate >= startdate);
            if (enddate != DateTime.MinValue)
                registers = registers.Where(x => x.RegisterDate <= enddate);

            var listModel = new RegisterMaps().RegisterIndexMap(registers);

            var starttext = startdate == DateTime.MinValue ? "∞" : startdate.ToShortDateString();
            var endtext = enddate == DateTime.MinValue ? "∞" : enddate.ToShortDateString();

            ViewData["Title"] = $"{starttext} - {endtext} Arasındaki Kayıtlar";
            ViewData["Heading"] = $"{starttext} - {endtext} Arasındaki Kayıtlar";

            return View("Index", listModel);
        }

        [Authorize(Policy = "Kayit Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _registerService.GetRegistrationAsync(x => x.Id == id,
                                                        x => x.Student, x => x.Discount,
                                                        x => x.Course, x => x.Course.Season);
            if (registration == null)
            {
                return NotFound();
            }

            var model = new RegisterMaps().RegisterDetailsMap(registration);

            return View(model);
        }

        [Authorize(Policy = "Kayit Ekle")]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterFormModel
            {
                RegisterDate = DateTime.Now
            };


            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            ViewData["DiscountId"] = new SelectList(await _discountService.GetAllAsync(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetCourses(Guid studentid)
        {
            var allCourses = await _courseService.GetAllAsync(x =>
                x.CoursesEnded == false &&
                !x.Registrations.Any(y => y.StudentId == studentid && y.LeaveDate == null));
            var student = await _studentService.GetAsync(x => x.Id == studentid);

            SelectList obgcity = CourseSelects.CourseNameDayList(allCourses);
            var result = new { data = obgcity, data2 = student.DiscountId };
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetGroups(Guid courseid)
        {
            var course = await _courseService.GetCourseAsync(x => x.Id == courseid, x => x.Groups);


            SelectList obgcity =  new SelectList(course.Groups, "Id", "Name");
            var result = new { data = obgcity };
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetDates(Guid courseid, DateTime startdate)
        {
            var course = await _courseService.GetCourseAsync(x => x.Id == courseid,x=> x.Groups);
            if (course.Groups.Count > 0)
            {
                ViewData["GroupId"] = new SelectList(course.Groups, "Id", "Name");
            }

            var dateList = new List<DateTime>();
            dateList = _courseService.CourseLessonDates(course);

            var lessonCount = dateList.Where(x => x.Date > startdate).Count();
            ViewData["Single"] = course.OneLessonPrice.ToString();
            ViewData["LessonCount"] = lessonCount.ToString();
            ViewData["TotalCount"] = (course.OneLessonPrice * lessonCount).ToString();

            var model = new DateListViewModel()
            {
                Dates = new List<DateTime>(),
                PaidDates = new List<DateTime>(),
                StartOfRegister = dateList.First(x => x.Date >= startdate)
            };
            model.Dates = dateList; 
            return PartialView("_DateList", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var student = await _studentService.GetAsync(x => x.Id == model.StudentId, y => y.Discount);
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Discount discount = null;

                if (model.DiscountId != null)
                    discount = await _discountService.GetDiscountAsync(x => x.Id == model.DiscountId);


                var course = await _courseService.GetCourseAsync(x => x.Id == model.CourseId);
                var count = _courseService.NumberOfLessonsForStudent(course, model.StartToCourseDate);

                var totalPrice = _registerService.CalculatePrice(course, count);

                totalPrice = _discountService.ApplyDiscount(discount, totalPrice);

                var register = new Registration()
                {
                    Course = course,
                    Student = student,
                    RegisterDate = model.RegisterDate,
                    StartToCourseDate = model.StartToCourseDate,
                    Price = totalPrice / count,
                    PaymentLeft = totalPrice,
                    NrOfLessonStudentWillJoin = count,
                    Discount = discount,
                    AppUserId = user.Id,
                    GroupId = model.GroupId
                };
                _registerService.AddRegistration(register);
                student.Balance += totalPrice;

                var changes = await _save.Completeasync("Yeni kayıt oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni kayıt oluşturuldu";

                return RedirectToAction("details", "Ogrenci", new { id = student.Id });
            }

            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            ViewData["DiscountId"] = new SelectList(await _discountService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [Authorize(Policy = "Kayit Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _registerService.GetRegistrationAsync(x => x.Id == id, x=> x.Course, x=> x.Course.Groups);
            if (registration == null)
            {
                return NotFound();
            }
            var model = new RegisterFormModel()
            {
                Id = registration.Id,
                GroupId = registration.GroupId
            };
            ViewData["GroupId"] = new SelectList(registration.Course.Groups, "Id", "Name");
            ViewData["Title"] = "Kayıt Düzenle";
            ViewData["Heading"] = "Kayıt Düzenle";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterFormModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var register = await _registerService.GetRegistrationAsync(x => x.Id == model.Id);
                register.GroupId = model.GroupId;

                var changes = await _save.Completeasync("Kayıt düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Kayıt düzenlendi";


                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [Authorize(Policy = "Kayit Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registration = await _registerService.GetRegistrationAsync(x => x.Id == id,
                                                                x => x.Student, x => x.Course, x => x.Course.Season);
            if (registration == null)
            {
                return NotFound();
            }
            var model = new RegisterViewModel()
            {
                Id = registration.Id,
                StudentName = registration.Student.FullName,
                StudentId = registration.StudentId,
                CourseName = registration.Course.Name,
                CourseId = registration.CourseId,
                SeasonName = registration.Course.Season.Name,
                Price = registration.Price,
                PaymentLeft = registration.PaymentLeft,
                RegisterDate = registration.StartToCourseDate
            };

            return View(model);
        }

        // POST: Registrations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var register = await _registerService.GetRegistrationAsync(x => x.Id == id, x => x.Student);

            register.Student.Balance -= register.PaymentLeft;

            _registerService.DeleteRegistration(register);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Kayıt silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Kayıt silindi";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<ActionResult> GetRegisters(Guid studentid, bool giveback)
        {
            List<SelectListItem> registerSelectList = new List<SelectListItem>();
            if (!giveback)
            {
                var allRegisters = await _registerService.GetAllAsync(x => x.StudentId == studentid && x.LeaveDate == null, null, x => x.Course, x=> x.Group);

                registerSelectList = new Selector().RegisterSelect(allRegisters);
            }
            if (giveback)
            {
                var allRegisters = await _registerService.GetAllAsync(x => x.StudentId == studentid, null, x => x.Course,x=> x.Group);

                registerSelectList = new Selector().RegisterSelect(allRegisters);
            }


            var result = new { data = registerSelectList };
            return Json(result);
        }


    }
}
