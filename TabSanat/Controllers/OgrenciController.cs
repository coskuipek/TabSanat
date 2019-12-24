using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class OgrenciController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IDiscountService _discountService;
        private readonly IExtraService _extraService;
        private readonly IRegisterService _registerService;
        private readonly ISaleService _saleService;
        private readonly ISaveService _save;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<AppUser> _userManager;

        public OgrenciController(IHostingEnvironment hostingEnvironment, IStudentService studentService, ICourseService courseService, IDiscountService discountService, IExtraService extraService, IRegisterService registerService, ISaleService saleService, ISaveService save, IPaymentService paymentService, UserManager<AppUser> userManager)
        {
            _hostingEnvironment = hostingEnvironment;
            _studentService = studentService;
            _courseService = courseService;
            _discountService = discountService;
            _extraService = extraService;
            _registerService = registerService;
            _saleService = saleService;
            _save = save;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        [Authorize(Policy = "Ogrenci Liste")]
        public async Task<IActionResult> Index()
        {
            var students = await _studentService.GetAllAsync();

            var model = StudentMaps.StudentIndexMap(students);

            ViewData["Title"] = "Tüm Öğrenciler";
            ViewData["Heading"] = "Tüm Öğrenciler";

            return View(model);
        }

        [Authorize(Policy = "Ogrenci Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var student = await _studentService.GetAsync(x => x.Id == id.Value, x => x.Discount);

            if (student == null)
                return NotFound();

            var model = StudentMaps.StudentDetailsMap(student);

            model.Registers = new List<RegisterViewModel>();
            var registers = await _registerService.GetAllAsync(x => x.Student == student,
                                                    y => y.OrderByDescending(z => z.StartToCourseDate),
                                                    x => x.Course, y => y.Course.Season, x=> x.Group);
            foreach (var register in registers)
            {
                var registerModel = new RegisterViewModel()
                {
                    Id = register.Id,
                    RegisterDate = register.StartToCourseDate,
                    SeasonName = register.Course.Season.Name,
                    CourseId = register.CourseId,
                    CourseName = register.Group == null ? 
                        $"{register.Course.Name} {Translator.DayName(register.Course.DayOfWeek)}" : 
                        $"{register.Course.Name} {Translator.DayName(register.Course.DayOfWeek)} {register.Group.Name}",
                    PriceDisplay = $"{register.Price} x {register.NrOfLessonStudentWillJoin}",
                    LeaveDate = register.LeaveDate,
                    PaymentLeft = register.PaymentLeft

                };
                model.Registers.Add(registerModel);
            }

            model.Sales = new List<SaleViewModel>();
            var sales = await _saleService.GetAllAsync(x => x.Student == student,
                                            y => y.OrderByDescending(z => z.Date),
                                            x => x.SaleItems, x => x.AppUser);
            foreach (var sale in sales)
            {
                var saleModel = new SaleViewModel()
                {
                    Id = sale.Id,
                    DateOfSale = sale.Date,
                    TotalPrice = sale.TotalPrice,
                    AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
                };

                foreach (var item in sale.SaleItems)
                {
                    var extra = await _extraService.GetExtraAsync(x => x.Id == item.ExtraId);
                    var itemModel = new SaleItemViewModel()
                    {
                        ExtraName = extra.Name,
                        Amount = item.Amount,
                        PriceEach = item.PriceEach
                    };
                    saleModel.SaleItems.Add(itemModel);
                }

                model.Sales.Add(saleModel);
            }


            model.Payments = new List<PaymentViewModel>();
            var payments = await _paymentService.GetAllAsync(x => x.StudentId == student.Id,
                                            y => y.OrderByDescending(z => z.PaymentDate),
                                            x => x.PaymentType, x => x.AppUser, x=> x.Registration, x=> x.Registration.Course);
            foreach (var payment in payments)
            {
                var modelPayment = new PaymentViewModel()
                {
                    Id = payment.Id,
                    PaymentDate = payment.PaymentDate,
                    Price = payment.IsGiveBack ? payment.Price *= -1 : payment.Price,
                    PaymentTypeName = payment.PaymentType.Name,
                    UserName = $"{payment.AppUser.FirstName} {payment.AppUser.LastName}",
                    Taksit = payment.Taksit,
                    CourseName = $"{payment.Registration.Course.Name} {Translator.DayName(payment.Registration.Course.DayOfWeek)}"
                };
                model.Payments.Add(modelPayment);
            }

            return View(model);
        }

        [Authorize(Policy = "Ogrenci Ekle")]
        public async Task<IActionResult> Yeni()
        {
            var model = new StudentFormModel();

            ViewData["ExtraId"] = new SelectList(await _extraService.GetAllAsync(), "Id", "Name");
            ViewData["CourseId"] = new SelectList(await _courseService.GetAllAsync(x => x.CoursesEnded == false), "Id", "Name");
            ViewData["DiscountId"] = new SelectList(await _discountService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Yeni(StudentFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string uniqueFileName = await StudentPhotoUpload(model);
                Discount discount = null;

                if (model.DiscountOfStudent.Id != null)
                    discount = await _discountService.GetDiscountAsync(x => x.Id == model.DiscountOfStudent.Id);


                var student = StudentMaps.StudentCreateMap(model, user, discount, uniqueFileName);
                _studentService.SaveToDatabase(student);


                //if (model.AssignToCourses != null)
                //{
                //    foreach (var courseId in model.AssignToCourses)
                //    {
                //        var course = await _courseService.GetCourseAsync(x => x.Id == courseId);
                //        var count = _courseService.NumberOfLessonsForStudent(course, model.StartToCourseDate);

                //        var totalPrice = _registerService.CalculatePrice(course, count);

                //        totalPrice = _discountService.ApplyDiscount(discount, totalPrice);

                //        var register = new Registration()
                //        {
                //            Course = course,
                //            Student = student,
                //            RegisterDate = model.RegisterDate,
                //            StartToCourseDate = model.StartToCourseDate,
                //            Price = totalPrice / count,
                //            PaymentLeft = totalPrice,
                //            NrOfLessonStudentWillJoin = count,
                //            Discount = discount,
                //            AppUserId = user.Id
                //        };
                //        _registerService.AddRegistration(register);
                //        student.Balance += totalPrice;
                //    }
                //}

                var changes = await _save.Completeasync("Yeni öğrenci oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni öğrenci oluşturuldu";
                return RedirectToAction("details", new { id = student.Id });
            }
            return View(model);
        }

        [Authorize(Policy = "Ogrenci Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var student = await _studentService.GetAsync(x => x.Id == id.Value, x => x.Discount);
            if (student == null)
            {
                return NotFound();
            }

            StudentFormModel model = new StudentFormModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                FullName = student.FullName,
                BirthDate = student.BirthDate,
                RegisterDate = student.RegisterDate,
                PhotoPath = student.PhotoPath,
                PhoneNo = student.PhoneNo,
                Email = student.Email,
                Address = student.Address,
                TcKimlikNo = student.TcKimlikNo,
                FatherFullName = student.FatherFullName,
                FatherPhoneNo = student.FatherPhoneNo,
                FatherJob = student.FatherJob,
                MotherFullName = student.MotherFullName,
                MotherPhoneNo = student.MotherPhoneNo,
                MotherJob = student.MotherJob,
                Institution = student.Institution,
                Balance = student.Balance

            };

            model.DiscountOfStudent = new DiscountViewModel();
            if (student.DiscountId != null)
            {
                model.DiscountOfStudent.Id = student.Discount.Id;
                model.DiscountOfStudent.Name = student.Discount.Name;
                model.DiscountOfStudent.AmountOfDiscount = student.Discount.AmountOfDiscount;
            }

            ViewData["DiscountId"] = new SelectList(await _discountService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var student = await GetStudent(model.Id);

                student.FirstName = _save.FixName(model.FirstName);
                student.LastName = _save.FixName(model.LastName);
                student.BirthDate = model.BirthDate;
                student.RegisterDate = model.RegisterDate;
                student.PhoneNo = model.PhoneNo;
                student.Email = model.Email;
                student.Address = model.Address;
                student.TcKimlikNo = model.TcKimlikNo;
                student.Institution = _save.FixName(model.Institution);
                student.FatherFullName = _save.FixName(model.FatherFullName);
                student.FatherPhoneNo = model.FatherPhoneNo;
                student.FatherJob = model.FatherJob;
                student.MotherFullName = _save.FixName(model.MotherFullName);
                student.MotherPhoneNo = model.MotherPhoneNo;
                student.MotherJob = model.MotherJob;
                student.Balance = model.Balance;
                student.DiscountId = model.DiscountOfStudent.Id;
                if (model.Photo != null)
                {
                    _studentService.DeleteStudentsPhoto(model.PhotoPath, _hostingEnvironment.WebRootPath);

                    student.PhotoPath = await StudentPhotoUpload(model);
                }

                try
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User);
                    var changes = await _save.Completeasync("Öğrenci kaydı düzenlendi", user);
                    if (changes > 0)
                        TempData["SMessage"] = $"Öğrenci kaydı düzenlendi";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_studentService.StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Ogrenci Kurstan Çıkar")]
        public async Task<IActionResult> KurstanCikar()
        {
            var model = new KurstanCikarFormModel();

            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KurstanCikar(KurstanCikarFormModel model)
        {
            if (ModelState.IsValid)
            {
                var register = await _registerService.GetRegistrationAsync(x => x.Id == model.Id, x => x.Course);
                var student = await GetStudent(register.StudentId);
                if (model.LeaveDate == new DateTime(3000, 12, 12))
                {
                    student.Balance -= register.PaymentLeft;
                    register.PaymentLeft = 0;
                    register.LeaveDate = DateTime.Now;
                }
                else
                {
                    var allDates = _courseService.CourseLessonDates(register.StartToCourseDate, register.Course.EndDate, register.Course.DayOfWeek);

                    var joinedDates = allDates.Where(x => x.Date <= model.LeaveDate);
                    var totalPrice = register.NrOfLessonStudentWillJoin * register.Price;
                    var checkPrice = joinedDates.Count() * register.Price;
                    var requiredPaymentLeft = totalPrice - checkPrice;


                    if (requiredPaymentLeft < register.PaymentLeft)
                    {
                        ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
                        var requiredPayment = register.PaymentLeft - requiredPaymentLeft;
                        TempData["EMessage"] = $"Öğrencinin kaydının silinmesi için {requiredPayment} TL gerekiyor.";
                        return RedirectToAction("KurstanCikar");
                    }

                    var notJoinedDates = allDates.Where(x => x.Date > model.LeaveDate);
                    var courseDatesCount = notJoinedDates.Count();
                    var singleLessonPrice = register.Price;

                    var changeAmount = courseDatesCount * singleLessonPrice;
                    student.Balance -= changeAmount;
                    register.PaymentLeft -= changeAmount;
                    register.LeaveDate = model.LeaveDate;
                }
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var changes = await _save.Completeasync($"{student.FullName} - {register.Course.Name} kaydı silindi.", user);
                if (changes > 0)
                    TempData["SMessage"] = $"{student.FullName} - {register.Course.Name} kaydı silindi.";

                return RedirectToAction("details", new { id = student.Id });
            }

            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            return View(model);
        }


        [Authorize(Policy = "Ogrenci Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var student = await GetStudent(id.Value);
            if (student == null)
                return NotFound();

            var model = new StudentViewModel()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                RegisterDate = student.RegisterDate
            };


            return View(model);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var student = await GetStudent(id);
            _studentService.DeleteStudentsPhoto(student.PhotoPath, _hostingEnvironment.WebRootPath);

            _studentService.RemoveFromDatabase(student);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Öğrenci silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Öğrenci silindi";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> StudentPhotoUpload(StudentFormModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }
        private async Task<Student> GetStudent(Guid id)
        {
            var student = await _studentService.GetAsync(x => x.Id == id);
            return student;
        }

        [Authorize(Policy = "Ogrenci Ayrilanlar")]
        public async Task<IActionResult> Ayrilanlar()
        {
            var students = await _studentService.GetAllAsync(
                x => x.Registers.Count > 0 &&
                !x.Registers.Any(y => y.LeaveDate == null));

            var model = StudentMaps.StudentIndexMap(students);

            ViewData["Title"] = "Ayrılan Öğrenciler";
            ViewData["Heading"] = "Ayrılan Öğrenciler";

            return View("Index", model);
        }
    }
}
