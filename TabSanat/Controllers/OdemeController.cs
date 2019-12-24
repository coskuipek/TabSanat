using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Maps;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class OdemeController : Controller
    {

        private readonly IPaymentService _paymentService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly ISaveService _save;
        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IExtraService _extraService;
        private readonly UserManager<AppUser> _userManager;

        private readonly IRegisterService _registerService;

        public OdemeController(IPaymentService paymentService, IPaymentTypeService paymentTypeService, ISaveService save, IStudentService studentService, ICourseService courseService, IExtraService extraService, UserManager<AppUser> userManager, IRegisterService registerService)
        {
            _paymentService = paymentService;
            _paymentTypeService = paymentTypeService;
            _save = save;
            _studentService = studentService;
            _courseService = courseService;
            _extraService = extraService;
            _userManager = userManager;
            _registerService = registerService;
        }


        [Authorize(Policy = "Odeme Liste")]
        public async Task<IActionResult> Index()
        {
            var payments = await _paymentService.GetAllAsync(x=> x.TimeToShow < DateTime.Now.AddDays(1), null,
                                x => x.PaymentType, y => y.Student, x => x.AppUser,
                                x => x.Registration, x => x.Registration.Course);

            var listModel = PaymentMaps.PaymentIndexMap(payments);

            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Heading"] = "Tüm Ödemeler";
            ViewData["Title"] = "Tüm Ödemeler";

            return View(listModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarihFiltre(DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser, bool showfuture)
        {
            var payments = await _paymentService.GetAllAsync(null, null,
                                     x => x.PaymentType, y => y.Student, x => x.AppUser,
                                     x => x.Registration, x => x.Registration.Course);

            var filteredPayments = await _paymentService.FilterPayments(payments, startdate, enddate, paymenttype, appuser, showfuture);

            var listModel = PaymentMaps.PaymentIndexMap(filteredPayments);

            var starttext = startdate == DateTime.MinValue ? "∞" : startdate.ToShortDateString();
            var endtext = enddate == DateTime.MinValue ? "∞" : enddate.ToShortDateString();

            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Title"] = $"{starttext} - {endtext} Arasındaki Ödemeler";
            ViewData["Heading"] = $"{starttext} - {endtext} Arasındaki Ödemeler";

            return View("Index", listModel);
        }

        [Authorize(Policy = "Odeme Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var payment = await _paymentService.GetPaymentAsync(x => x.Id == id,
                            x => x.PaymentType, x => x.Student, x => x.AppUser);

            var register = await _registerService.GetRegistrationAsync(x => x.Id == payment.RegistrationId, x => x.Course, x => x.Course.Season);

            if (payment == null || register == null)
                return NotFound();

            var model = new PaymentViewModel()
            {
                PaymentDate = payment.PaymentDate,
                Price = payment.Price,
                UserName = $"{payment.AppUser.FirstName} {payment.AppUser.LastName}",
                PaymentTypeName = payment.PaymentType.Name,
                AmountOfLessons = Convert.ToInt32(payment.Price / register.Price)

            };
            if (payment.Student != null)
            {
                model.Student = new StudentViewModel()
                {
                    FullName = payment.Student.FullName,
                    Address = payment.Student.Address,
                    PhoneNo = payment.Student.PhoneNo
                };
            }



            model.Register = new RegisterViewModel()
            {
                CourseName = register.Course.Name,
                DayOfWeek = new System.Globalization.CultureInfo("tr-TR").DateTimeFormat.DayNames[(int)register.Course.DayOfWeek],
                SeasonName = register.Course.Season.Name,
                Price = register.Price
            };

            return View(model);
        }

        [Authorize(Policy = "Odeme Ekle")]
        public async Task<IActionResult> Create()
        {
            var model = new PaymentFormModel();

            model.PaymentDate = DateTime.Now;
            model.IsGiveBack = false;

            var types = await GetAllPaymentTypes();
            model.PaymentTypes = types.ToList();

            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName");

            ViewData["Title"] = "Kurs Ödemesi";
            ViewData["Heading"] = "Kurs Ödemesi";

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentFormModel model)
        {
            if (model.Price <= 0M)
                ModelState.AddModelError("TotalPrice", "Miktar 0 veya 0'dan küçük olamaz.");
            if (model.PaymentTypeId == null || model.PaymentTypeId == Guid.Empty)
                ModelState.AddModelError("PaymentTypeId", "Hiçbir ödeme tipi seçilmedi.");

            if (model.IsGiveBack == true)
                return NotFound();


            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                if (model.Taksit == 1)
                {
                    var payment = new Payment()
                    {
                        PaymentDate = model.PaymentDate,
                        TimeToShow = model.PaymentDate,
                        PaymentTypeId = model.PaymentTypeId,
                        StudentId = model.StudentId,
                        Price = model.Price,
                        AppUserId = user.Id,
                        Taksit = "Yok"
                    };

                    var register = await _registerService.GetRegistrationAsync(x => x.Id == model.RegisterId, x => x.Student);

                    _paymentService.SavePaymentToDatabase(payment, register);
                }
                else if (model.Taksit > 1)
                {
                    var register = await _registerService.GetRegistrationAsync(x => x.Id == model.RegisterId, x => x.Student);

                    for (int i = 0; i < model.Taksit; i++)
                    {
                        var payment = new Payment()
                        {
                            PaymentDate = model.PaymentDate,
                            TimeToShow = model.PaymentDate.AddMonths(i),
                            PaymentTypeId = model.PaymentTypeId,
                            StudentId = model.StudentId,
                            Price = model.Price / model.Taksit,
                            AppUserId = user.Id,
                            Taksit = $"({i+1}/{model.Taksit})"
                        };
                        _paymentService.SavePaymentToDatabase(payment, register);
                    }
                }
                

                var changes = await _save.Completeasync("Yeni ödeme kaydedildi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni ödeme kaydedildi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(await GetAllPaymentTypes(), "Id", "Name", model.PaymentTypeId);
            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> GetRemainingPrice(Guid registerid)
        {
            var register = await _registerService.GetRegistrationAsync(x => x.Id == registerid, x => x.Course);
            
            var singleRegisterPrice = register.Price;

            var totalRemainingForCourse = register.PaymentLeft;
            var allLessonCount = totalRemainingForCourse / singleRegisterPrice;
            if (allLessonCount < 0)
                allLessonCount *= -1;

            var nrOfLate = _courseService.NumberOfUnpaidLessons(register);

            var priceExpected = register.Price + nrOfLate * register.Price;



            var list = new List<string>();

            list.Add(totalRemainingForCourse.ToString());
            list.Add(singleRegisterPrice.ToString());
            if (totalRemainingForCourse == 0)
            {
                list.Add("Ödeme tamamlandı.");
            }
            else
            {
                list.Add(priceExpected.ToString());
            }
            list.Add(nrOfLate.ToString());
            list.Add(allLessonCount.ToString());

            var result = new { data = list };

            return Json(result);
        }

        [Authorize(Policy = "Odeme Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            var payment = await _paymentService.GetPaymentAsync(x => x.Id == id);

            if (payment == null)
                return NotFound();

            var model = new PaymentFormModel()
            {
                Id = payment.Id,
                Price = payment.Price,
                PaymentDate = payment.PaymentDate
            };

            ViewData["PaymentTypeId"] = new SelectList(await GetAllPaymentTypes(), "Id", "Name", payment.PaymentTypeId);
            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName", payment.StudentId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentFormModel model)
        {
            if (id != model.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var payment = await _paymentService.GetPaymentAsync(x => x.Id == model.Id);

                if (payment.Price != model.Price)
                {
                    var student = await _studentService.GetAsync(x => x.Id == payment.StudentId);

                    if (payment.Price > model.Price)
                        student.Balance += payment.Price - model.Price;

                    else
                        student.Balance -= model.Price - payment.Price;

                    payment.Price = model.Price;
                    payment.PaymentDate = model.PaymentDate;
                    payment.PaymentTypeId = model.PaymentTypeId;
                }
                var changes = await _save.Completeasync("Ödeme düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Ödeme düzenlendi";


                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(await GetAllPaymentTypes(), "Id", "Name", model.PaymentTypeId);
            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName", model.StudentId);
            return View(model);
        }

        [Authorize(Policy = "Odeme Iade")]
        public async Task<IActionResult> Iade()
        {
            var model = new PaymentFormModel
            {
                IsGiveBack = true,
                PaymentDate = DateTime.Now
            };

            var types = await GetAllPaymentTypes();
            model.PaymentTypes = types.ToList();

            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName");

            ViewData["Title"] = "Kurs Ödeme İadesi";
            ViewData["Heading"] = "Kurs Ödeme İadesi";

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Iade(PaymentFormModel model)
        {
            if (model.Price <= 0M)
                ModelState.AddModelError("TotalPrice", "Miktar 0 veya 0'dan küçük olamaz.");

            if (model.IsGiveBack == false)
                return NotFound();

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var payment = new Payment()
                {
                    PaymentDate = model.PaymentDate,
                    PaymentTypeId = model.PaymentTypeId,
                    StudentId = model.StudentId,
                    Price = model.Price,
                    AppUserId = user.Id,
                    IsGiveBack = true
                };

                var register = await _registerService.GetRegistrationAsync(x => x.Id == model.RegisterId,
                                                                                            x => x.Student);
                _paymentService.SavePaymentToDatabase(payment, register);

                var changes = await _save.Completeasync("Yeni iade kaydedildi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni iade kaydedildi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PaymentTypeId"] = new SelectList(await GetAllPaymentTypes(), "Id", "Name", model.PaymentTypeId);
            ViewData["StudentId"] = new SelectList(await GetAllStudents(), "Id", "FullName");
            return View(model);
        }

        [Authorize(Policy = "Odeme Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var payment = await _paymentService.GetPaymentAsync(x => x.Id == id, x => x.PaymentType, x => x.Student);

            if (payment == null)
                return NotFound();

            var model = new PaymentViewModel()
            {
                Id = payment.Id,
                PaymentDate = payment.PaymentDate,
                StudentName = payment.Student.FullName,
                Price = payment.Price
            };

            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var payment = await _paymentService.GetPaymentAsync(x => x.Id == id);

            var register = await _registerService.GetRegistrationAsync(x => x.Id == payment.RegistrationId, x => x.Student);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _paymentService.DeletePaymentFromDatabase(payment, register);

            var changes = await _save.Completeasync("Ödeme bilgisi silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Ödeme bilgisi silindi";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IQueryable<PaymentType>> GetAllPaymentTypes()
        {
            return await _paymentTypeService.GetAllAsync();
        }
        private async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await _studentService.GetAllAsync();
        }
        public async Task<IActionResult> CheckAmount(decimal totalprice, Guid studentId, Guid courseId)
        {
            if (totalprice <= 0)
                return Json(data: $"Ödeme miktarı 0 veya 0'dan küçük olamaz.");

            var student = await _studentService.GetAsync(x => x.Id == studentId, x => x.Registers);


            //TEK KURS İÇİN ÖDEME MİKTARI   
            var priceLeft = student.Registers.FirstOrDefault(x => x.CourseId == courseId).PaymentLeft;

            if (totalprice > priceLeft)
                return Json(data: $"Maksimum ödeme {priceLeft} olabilir");

            else
                return Json(data: true);
        }

        [HttpPost]
        public async Task<ActionResult> GetDates(Guid registerid, int lessonsToPay)
        {
            var register = await _registerService.GetRegistrationAsync(x => x.Id == registerid, x => x.Course);

            var dateList = new List<DateTime>();
            dateList = _courseService.CourseLessonDates(register.Course);

            var model = new DateListViewModel()
            {
                StartOfRegister = dateList.First(x => x.Date >= register.StartToCourseDate),
                Dates = new List<DateTime>(),
                PaidDates = new List<DateTime>(),
                IncomingPaidDates = new List<DateTime>()
            };
            model.Dates = dateList.SkipWhile(x=> x.Date < register.StartToCourseDate).ToList();


            var totalPriceForAllCourse = register.NrOfLessonStudentWillJoin * register.Price;
            var amountOfLessonsPaid = (totalPriceForAllCourse - register.PaymentLeft) / register.Price;

            model.PaidDates = dateList.Where(x => x.Date >= register.StartToCourseDate).Take((int)amountOfLessonsPaid).ToList();
            model.IncomingPaidDates = dateList.Where(x => x.Date >= register.StartToCourseDate).Skip((int)amountOfLessonsPaid)
                .Take(lessonsToPay).ToList();

            return PartialView("_DateList", model);
        }

        
    }
}
