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
    public class IndirimController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public IndirimController(IDiscountService discountService, UserManager<AppUser> userManager, ISaveService save)
        {
            _discountService = discountService;
            _userManager = userManager;
            _save = save;
        }

        [Authorize(Policy = "Indirim Liste")]
        public async Task<IActionResult> Index()
        {
            var discounts = await _discountService.GetAllAsync();

            var modelList = new List<DiscountViewModel>();

            foreach (var discount in discounts)
            {
                var model = new DiscountViewModel()
                {
                    Id = discount.Id,
                    Name = discount.Name,
                    AmountOfDiscount = discount.AmountOfDiscount,
                    IsFixedAmount = discount.IsFixedAmount
                };
                modelList.Add(model);
            }

            return View(modelList);
        }

        [Authorize(Policy = "Indirim Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var discount = await _discountService.GetDiscountAsync(x => x.Id == id);
            if (discount == null)
            {
                return NotFound();
            }
            var model = new DiscountViewModel()
            {
                Id = discount.Id,
                Name = discount.Name,
                AmountOfDiscount = discount.AmountOfDiscount
            };

            return View(model);
        }

        [Authorize(Policy = "Indirim Ekle")]
        public IActionResult Create()
        {
            DiscountFormModel model = new DiscountFormModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                Discount discount = new Discount()
                {
                    Name = _save.FixName(model.Name),
                    AmountOfDiscount = model.AmountOfDiscount,
                    IsFixedAmount = model.IsFixedAmount,
                    OnlyOnce = model.OnlyOnce
                };

                _discountService.AddDiscount(discount);

                var changes = await _save.Completeasync("İndirim tipi oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"İndirim tipi oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Indirim Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var discount = await _discountService.GetDiscountAsync(x => x.Id == id);
            if (discount == null)
            {
                return NotFound();
            }
            var model = new DiscountFormModel
            {
                Id = discount.Id,
                Name = discount.Name,
                AmountOfDiscount = discount.AmountOfDiscount,
                IsFixedAmount = discount.IsFixedAmount,
                OnlyOnce = discount.OnlyOnce
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DiscountFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var discount = await _discountService.GetDiscountAsync(x => x.Id == model.Id);

                discount.Name = _save.FixName(model.Name);
                discount.AmountOfDiscount = model.AmountOfDiscount;
                discount.IsFixedAmount = model.IsFixedAmount;
                discount.OnlyOnce = model.OnlyOnce;

                var changes = await _save.Completeasync("İndirim düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"İndirim düzenlendi";


                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Indirim Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var discount = await _discountService.GetDiscountAsync(x => x.Id == id);
            if (discount == null)
            {
                return NotFound();
            }
            var model = new DiscountViewModel()
            {
                Id = discount.Id,
                Name = discount.Name,
                AmountOfDiscount = discount.AmountOfDiscount
            };

            return View(model);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _discountService.DeleteDiscount(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("İndirim silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"İndirim silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
