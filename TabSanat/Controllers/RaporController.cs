using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;

namespace TabSanat.Controllers
{
    [Authorize]
    public class RaporController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly UserManager<AppUser> _userManager;

        public RaporController(ISaleService saleService, IPaymentService paymentService, IPaymentTypeService paymentTypeService, UserManager<AppUser> userManager)
        {
            _saleService = saleService;
            _paymentService = paymentService;
            _paymentTypeService = paymentTypeService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Gelen()
        {
            var modelList = new List<IncomingViewModel>();

            var sales = await _saleService.GetAllAsync(null, null,
                                    x => x.PaymentType, x => x.Student, x => x.AppUser);

            foreach (var sale in sales)
            {
                var model = new IncomingViewModel()
                {
                    Id = sale.Id,
                    Type = "Satış",
                    Date = sale.Date,
                    StudentId = sale.StudentId,
                    StudentName = sale.Student == null ? "" : sale.Student.FullName,
                    PaymentTypeName = sale.PaymentType.Name,
                    TotalPrice = sale.TotalPrice,
                    AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
                };
                modelList.Add(model);
            }

            var payments = await _paymentService.GetAllAsync(x => x.TimeToShow < DateTime.Now.AddDays(1), null,
                                    x => x.PaymentType, x => x.Student, x => x.AppUser);

            foreach (var payment in payments)
            {
                var model = new IncomingViewModel()
                {
                    Id = payment.Id,
                    Type = payment.IsGiveBack ? "İade" : "Kurs Ödemesi",
                    Date = payment.PaymentDate,
                    StudentId = payment.StudentId,
                    StudentName = payment.Student.FullName,
                    PaymentTypeName = payment.PaymentType.Name,
                    TotalPrice = payment.IsGiveBack ? payment.Price *= -1 : payment.Price,
                    AppUserName = $"{payment.AppUser.FirstName} {payment.AppUser.LastName}",
                    Taksit = payment.Taksit
                };
                modelList.Add(model);
            }

            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Heading"] = "Gelen Para Raporu";
            ViewData["Title"] = "Gelen Para Raporu";

            return View(modelList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarihFiltre(DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser, bool showfuture)
        {

            var modelList = new List<IncomingViewModel>();

            var payments = await _paymentService.GetAllAsync(null, null,
                         x => x.PaymentType, y => y.Student, x => x.AppUser);

            //FILTER
            var filteredPayments = await _paymentService.FilterPayments(payments, startdate, enddate, paymenttype, appuser, showfuture);
            //
            foreach (var payment in filteredPayments)
            {
                IncomingViewModel model = new IncomingViewModel()
                {
                    Id = payment.Id,
                    Type = "Kurs Ödemesi",
                    Date = payment.PaymentDate,
                    StudentId = payment.StudentId,
                    StudentName = payment.Student.FullName,
                    PaymentTypeName = payment.PaymentType.Name,
                    TotalPrice = payment.Price,
                    AppUserName = $"{payment.AppUser.FirstName} {payment.AppUser.LastName}",
                    Taksit = payment.Taksit
                };
                modelList.Add(model);
            }

            var sales = await _saleService.GetAllAsync(null, null,
                            x => x.PaymentType, y => y.Student, x => x.AppUser);

            var filteredSales = await _saleService.FilterSales(sales, startdate, enddate, paymenttype, appuser);


            foreach (var sale in filteredSales)
            {
                IncomingViewModel model = new IncomingViewModel()
                {
                    Id = sale.Id,
                    Type = "Satış",
                    Date = sale.Date,
                    StudentId = sale.StudentId,
                    StudentName = sale.Student == null ? "" : sale.Student.FullName,
                    PaymentTypeName = sale.PaymentType.Name,
                    TotalPrice = sale.TotalPrice,
                    AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
                };
                modelList.Add(model);
            }

            var starttext = startdate == DateTime.MinValue ? "∞" : startdate.ToShortDateString();
            var endtext = enddate == DateTime.MinValue ? "∞" : enddate.ToShortDateString();

            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Title"] = $"{starttext} - {endtext} Arasındaki Ödemeler";
            ViewData["Heading"] = $"{starttext} - {endtext} Arasındaki Ödemeler";

            return View("Gelen", modelList);
        }
    }
}