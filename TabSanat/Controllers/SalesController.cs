using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabSanat.Helpers;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class SalesController : Controller
    {

        private readonly ISaleService _saleService;
        private readonly IExtraService _extraService;
        private readonly IStudentService _studentService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public SalesController(ISaleService saleService, IExtraService extraService, IStudentService studentService, IPaymentTypeService paymentTypeService, UserManager<AppUser> userManager, ISaveService save)
        {

            _saleService = saleService;
            _extraService = extraService;
            _studentService = studentService;
            _paymentTypeService = paymentTypeService;
            _userManager = userManager;
            _save = save;
        }

        [Authorize(Policy = "Satış Liste")]
        public async Task<IActionResult> Index()
        {
            var sales = await _saleService.GetAllAsync(null, null, x => x.PaymentType, x => x.Student, x => x.AppUser, x => x.SaleItems);

            var modelList = new List<SaleViewModel>();

            foreach (var sale in sales)
            {
                var model = new SaleViewModel()
                {
                    Id = sale.Id,
                    DateOfSale = sale.Date,
                    StudentId = sale.StudentId,
                    StudentName = sale.Student == null ? "" : sale.Student.FullName,
                    PaymentTypeName = sale.PaymentType.Name,
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
                    model.SaleItems.Add(itemModel);
                }
                modelList.Add(model);
            }
            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Heading"] = "Tüm Satışlar";
            ViewData["Title"] = "Tüm Satışlar";

            return View(modelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TarihFiltre(DateTime startdate, DateTime enddate, Guid? paymenttype, string appuser)
        {
            List<SaleViewModel> listModel = new List<SaleViewModel>();

            var sales = await _saleService.GetAllAsync(null, null,
                            x => x.PaymentType, y => y.Student, x => x.AppUser);

            var filteredSales = await _saleService.FilterSales(sales, startdate, enddate, paymenttype, appuser);

            foreach (var sale in filteredSales)
            {
                SaleViewModel model = new SaleViewModel()
                {
                    Id = sale.Id,
                    DateOfSale = sale.Date,
                    StudentName = sale.Student == null ? "" : sale.Student.FullName,
                    StudentId = sale.StudentId,
                    PaymentTypeName = sale.PaymentType.Name,
                    TotalPrice = sale.TotalPrice,
                    AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
                };
                listModel.Add(model);
            }
            var starttext = startdate == DateTime.MinValue ? "∞" : startdate.ToShortDateString();
            var endtext = enddate == DateTime.MinValue ? "∞" : enddate.ToShortDateString();

            ViewData["AppUserId"] = new SelectList(await _userManager.Users.ToListAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["Title"] = $"{starttext} - {endtext} Arasındaki Satışlar";
            ViewData["Heading"] = $"{starttext} - {endtext} Arasındaki Satışlar";
            return View("Index", listModel);
        }

        [Authorize(Policy = "Satış Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _saleService.GetSaleAsync(x => x.Id == id,
                                                   x => x.AppUser, x => x.Student, x => x.PaymentType);
            if (sale == null)
            {
                return NotFound();
            }
            var model = new SaleViewModel()
            {
                Id = sale.Id,
                DateOfSale = sale.Date,
                StudentName = sale.Student == null ? "" : sale.Student.FullName,
                PaymentTypeName = sale.PaymentType.Name,
                TotalPrice = sale.TotalPrice,
                AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
            };

            return View(model);
        }

        [Authorize(Policy = "Satış Ekle")]
        public async Task<IActionResult> Create()
        {
            var model = new SaleFormModel()
            {
                DateOfSale = DateTime.Now
            };


            var extras = await _extraService.GetAllAsync();

            var extraSelect = new Selector().ExtraSelect(extras);


            ViewData["ExtraId"] = extraSelect;
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaleFormModel model)
        {
            if (ModelState.IsValid)
            {
                decimal totalCost = 0;
                List<SaleItem> saleList = new List<SaleItem>();

                var user = await _userManager.GetUserAsync(HttpContext.User);

                var sale = new Sale()
                {
                    Date = model.DateOfSale,
                    PaymentTypeId = model.PaymentTypeId,
                    StudentId = model.StudentId,
                    AppUser = user
                };

                sale.SaleItems = new List<SaleItem>();
                foreach (var modelExtra in model.ExtrasThatAreSold)
                {
                    var extra = await _extraService.GetExtraAsync(x => x.Id == modelExtra.Id);
                    sale.SaleItems.Add(new SaleItem
                    {
                        Extra = extra,
                        PriceEach = extra.PriceToSell,
                        Amount = modelExtra.Amount
                    });
                    totalCost += extra.PriceToSell * modelExtra.Amount;
                }
                sale.TotalPrice = totalCost;

                _saleService.SaveSaleToDatabase(sale);
                var changes = await _save.Completeasync("Yeni satış kaydedildi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni satış kaydedildi";

                return RedirectToAction("Index");
            }
            var extras = await _extraService.GetAllAsync();
            var extraSelect = new Selector().ExtraSelect(extras);

            ViewData["ExtraId"] = extraSelect;
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [Authorize(Policy = "Satış Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var sale = await _saleService.GetSaleAsync(x => x.Id == id);

            if (sale == null)
                return NotFound();


            var model = new SaleFormModel()
            {
                Id = sale.Id,
                StudentId = sale.StudentId,
                DateOfSale = sale.Date,
                PaymentTypeId = sale.PaymentTypeId,
                TotalPrice = sale.TotalPrice
            };

            ViewData["AppUserId"] = new SelectList(_userManager.Users, "Id", "FirstName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");

            return View(sale);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SaleFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var sale = await _saleService.GetSaleAsync(x => x.Id == model.Id);

                sale.AppUserId = model.AppUserId.ToString();
                sale.Date = model.DateOfSale;
                sale.PaymentTypeId = model.PaymentTypeId;
                sale.StudentId = model.StudentId;
                sale.TotalPrice = model.TotalPrice;

                var changes = await _save.Completeasync("Satış düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Satış düzenlendi";
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_userManager.Users, "Id", "FirstName");
            ViewData["PaymentTypeId"] = new SelectList(await _paymentTypeService.GetAllAsync(), "Id", "Name");
            ViewData["StudentId"] = new SelectList(await _studentService.GetAllAsync(), "Id", "FullName");
            return View(model);
        }

        [Authorize(Policy = "Satış Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sale = await _saleService.GetSaleAsync(x => x.Id == id,
                                                   x => x.AppUser, x => x.Student, x => x.PaymentType);
            if (sale == null)
            {
                return NotFound();
            }
            var model = new SaleViewModel()
            {
                Id = sale.Id,
                DateOfSale = sale.Date,
                StudentName = sale.Student == null ? "" : sale.Student.FullName,
                PaymentTypeName = sale.PaymentType.Name,
                TotalPrice = sale.TotalPrice,
                AppUserName = $"{sale.AppUser.FirstName} {sale.AppUser.LastName}"
            };

            return View(model);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sale = await _saleService.GetSaleAsync(x => x.Id == id);
            _saleService.DeleteSaleFromDatabase(sale);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var changes = await _save.Completeasync("Satış silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Satış silindi";
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> GetPrices(Guid extraid)
        {
            var extra = await _extraService.GetExtraAsync(x => x.Id == extraid);

            var result = new { data = extra.PriceToSell };
            return Json(result);
        }
    }
}
