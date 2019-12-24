using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;

namespace TabSanat.Controllers
{
    [Authorize]
    public class OdemeTipiController : Controller
    {

        private readonly IPaymentService _paymentService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public OdemeTipiController(IPaymentService paymentService, IPaymentTypeService paymentTypeService, UserManager<AppUser> userManager, ISaveService save)
        {
            _paymentService = paymentService;
            _paymentTypeService = paymentTypeService;
            _userManager = userManager;
            _save = save;
        }


        [Authorize(Policy = "OdemeTipi Liste")]
        public async Task<IActionResult> Index()
        {
            var types = await _paymentTypeService.GetAllAsync();

            var modelList = new List<PaymentTypeViewModel>();

            foreach (var type in types)
            {
                var model = new PaymentTypeViewModel()
                {
                    Id = type.Id,
                    Name = type.Name
                };
                modelList.Add(model);
            }

            return View(modelList);
        }

        [Authorize(Policy = "OdemeTipi Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var paymentType = await _paymentTypeService.GetTypeAsync(m => m.Id == id);
            if (paymentType == null)
            {
                return NotFound();
            }
            PaymentTypeViewModel model = new PaymentTypeViewModel()
            {
                Id = paymentType.Id,
                Name = paymentType.Name
            };

            var payments = await _paymentService.GetAllAsync(x => x.PaymentType == paymentType,
                                                        x => x.OrderByDescending(y => y.PaymentDate), x => x.Student);
            foreach (var payment in payments)
            {
                var modelItem = new PaymentViewModel()
                {
                    PaymentDate = payment.PaymentDate,
                    StudentName = payment.Student.FullName,
                    Price = payment.Price
                };
                model.Payments.Add(modelItem);
            }

            return View(model);
        }

        [Authorize(Policy = "OdemeTipi Ekle")]
        public IActionResult Create()
        {
            var model = new PaymentTypeFormModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentTypeFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var type = new PaymentType()
                {
                    Name = _save.FixName(model.Name)
                };
                _paymentTypeService.AddPaymentType(type);

                var changes = await _save.Completeasync("Yeni ödeme tipi oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni ödeme tipi oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            TempData["SMessage"] = "Kayıt Başarısız";
            return View(model);
        }

        [Authorize(Policy = "OdemeTipi Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var paymentType = await _paymentTypeService.GetTypeAsync(x => x.Id == id);
            if (paymentType == null)
            {
                return NotFound();
            }

            var model = new PaymentTypeFormModel()
            {
                Id = paymentType.Id,
                Name = paymentType.Name
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentTypeFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var paymentType = await _paymentTypeService.GetTypeAsync(x => x.Id == model.Id);

                paymentType.Name = _save.FixName(model.Name);


                var changes = await _save.Completeasync("Ödeme tipi düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Ödeme tipi düzenlendi";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "OdemeTipi Sil")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var paymentType = await _paymentTypeService.GetTypeAsync(x => x.Id == id);
            if (paymentType == null)
            {
                return NotFound();
            }

            var model = new PaymentTypeViewModel()
            {
                Id = paymentType.Id,
                Name = paymentType.Name
            };

            return View(model);
        }

        // POST: PaymentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            _paymentTypeService.DeletePaymentType(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var changes = await _save.Completeasync("Ödeme tipi silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Ödeme tipi silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
