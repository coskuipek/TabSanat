using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class ExtrasController : Controller
    {
        private readonly IExtraService _extraService;
        private readonly IStudentService _studentService;
        private readonly IPaymentService _paymentService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public ExtrasController(IExtraService extraService, IStudentService studentService, IPaymentService paymentService, IPaymentTypeService paymentTypeService, UserManager<AppUser> userManager, ISaveService save)
        {
            _extraService = extraService;
            _studentService = studentService;
            _paymentService = paymentService;
            _paymentTypeService = paymentTypeService;
            _userManager = userManager;
            _save = save;
        }
        [Authorize(Policy = "Ekstra Liste")]
        public async Task<IActionResult> Index()
        {
            var extras = await _extraService.GetAllAsync();

            var modelList = new List<ExtraViewModel>();

            foreach (var extra in extras)
            {
                var model = new ExtraViewModel()
                {
                    Id = extra.Id,
                    Name = extra.Name,
                    PriceToBuy = extra.PriceToBuy,
                    PriceToSell = extra.PriceToSell
                };
                modelList.Add(model);
            }

            return View(modelList);
        }

        [Authorize(Policy = "Ekstra Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();


            var extra = await _extraService.GetExtraAsync(m => m.Id == id);
            if (extra == null)
                return NotFound();

            var model = new ExtraViewModel()
            {
                Id = extra.Id,
                Name = extra.Name,
                PriceToBuy = extra.PriceToBuy,
                PriceToSell = extra.PriceToSell
            };

            model.SalesThatIncludeExtra = new List<SaleViewModel>();
            var sales = await _extraService.GetSalesOfExtra(extra);
            foreach (var sale in sales)
            {
                var saleModel = new SaleViewModel()
                {
                    StudentName = sale.Student.FullName,
                    DateOfSale = sale.Date,
                    TotalPrice = sale.TotalPrice
                };
                model.SalesThatIncludeExtra.Add(saleModel);
            }

            return View(model);
        }

        [Authorize(Policy = "Ekstra Ekle")]
        public IActionResult Create()
        {
            var model = new ExtraFormModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExtraFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var extra = new Extra()
                {
                    Name = _save.FixName(model.Name),
                    PriceToBuy = model.PriceToBuy,
                    PriceToSell = model.PriceToSell
                };
                _extraService.AddExtra(extra);

                var changes = await _save.Completeasync("Yeni ürün oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni ürün oluşturuldu";

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Ekstra Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var extra = await _extraService.GetExtraAsync(x => x.Id == id);
            if (extra == null)
            {
                return NotFound();
            }
            var model = new ExtraFormModel()
            {
                Id = extra.Id,
                Name = extra.Name,
                PriceToBuy = extra.PriceToBuy,
                PriceToSell = extra.PriceToSell
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ExtraFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var extra = await _extraService.GetExtraAsync(x => x.Id == model.Id);
                extra.Name = _save.FixName(model.Name);
                extra.PriceToBuy = model.PriceToBuy;
                extra.PriceToSell = model.PriceToSell;

                var changes = await _save.Completeasync("Ürün düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Ürün düzenlendi";


                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Ekstra Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var extra = await _extraService.GetExtraAsync(x => x.Id == id);

            if (extra == null)
            {
                return NotFound();
            }
            var model = new ExtraViewModel()
            {
                Id = extra.Id,
                Name = extra.Name,
                PriceToBuy = extra.PriceToBuy,
                PriceToSell = extra.PriceToSell
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _extraService.DeleteExtra(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Ürün silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Ürün silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
