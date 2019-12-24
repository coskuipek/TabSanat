using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class HarcamaController : Controller
    {

        private readonly IExpenseService _expenseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IExtraService _extraService;
        private readonly ISaveService _save;

        public HarcamaController(IExpenseService expenseService, UserManager<AppUser> userManager, IExtraService extraService, ISaveService save)
        {
            _expenseService = expenseService;
            _userManager = userManager;
            _extraService = extraService;
            _save = save;
        }



        [Authorize(Policy = "Harcama Liste")]
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllAsync(null, null, x => x.Extra, x => x.AppUser);

            var model = new List<ExpenseViewModel>();

            foreach (var expense in expenses)
            {
                var modelItem = new ExpenseViewModel()
                {
                    Id = expense.Id,
                    Name = expense.Name,
                    Date = expense.Date,
                    ExtraId = expense.ExtraId,
                    UserName = $"{expense.AppUser.FirstName} {expense.AppUser.LastName}",
                    Amount = expense.Amount,
                    PriceEach = expense.PriceEach
                };
                model.Add(modelItem);
            }


            return View(model);
        }

        [Authorize(Policy = "Harcama Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _expenseService.GetExpenseAsync(m => m.Id == id, x => x.AppUser, x => x.Extra);
            if (expense == null)
            {
                return NotFound();
            }

            var model = new ExpenseViewModel()
            {
                Id = expense.Id,
                Name = expense.Name,
                Date = expense.Date,
                ExtraId = expense.ExtraId,
                UserName = $"{expense.AppUser.FirstName} {expense.AppUser.LastName}",
                Amount = expense.Amount,
                PriceEach = expense.PriceEach
            };
            return View(model);
        }

        [Authorize(Policy = "Harcama Ekle")]
        public async Task<IActionResult> Create()
        {
            var model = new ExpenseFormModel()
            {
                Date = DateTime.Now
            };

            ViewData["ExtraId"] = new SelectList(await _extraService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var expense = new Expense()
                {
                    Date = model.Date,
                    AppUser = user,
                    Name = model.Name,
                    Amount = model.Amount,
                    PriceEach = model.PriceEach
                };

                if (model.ExtraId != null)
                {
                    var extra = await _extraService.GetExtraAsync(x => x.Id == model.ExtraId);
                    expense.Name = extra.Name;
                }

                _expenseService.AddExpenseToDatabase(expense);

                var changes = await _save.Completeasync("Harcama kaydedildi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Harcama kaydedildi";
                return RedirectToAction(nameof(Index));
            }

            ViewData["ExtraId"] = new SelectList(await _extraService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [Authorize(Policy = "Harcama Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _expenseService.GetExpenseAsync(x => x.Id == id);
            if (expense == null)
            {
                return NotFound();
            }
            var model = new ExpenseFormModel()
            {
                Id = expense.Id,
                Date = expense.Date,
                ExtraId = expense.ExtraId,
                Name = expense.Name,
                Amount = expense.Amount,
                PriceEach = expense.PriceEach
            };

            ViewData["ExtraId"] = new SelectList(await _extraService.GetAllAsync(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ExpenseFormModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var expense = await _expenseService.GetExpenseAsync(x => x.Id == model.Id);

                expense.Name = model.Name;
                expense.Date = model.Date;
                if (expense.ExtraId != model.ExtraId)
                {
                    var extra = await _extraService.GetExtraAsync(x => x.Id == model.ExtraId);
                    expense.Name = extra.Name;
                    expense.ExtraId = model.ExtraId;
                }

                expense.Amount = model.Amount;
                expense.PriceEach = model.PriceEach;

                var changes = await _save.Completeasync("Harcama düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Harcama düzenlendi";

                return RedirectToAction(nameof(Index));
            }

            ViewData["ExtraId"] = new SelectList(await _extraService.GetAllAsync(), "Id", "Name");
            return View(model);
        }

        [Authorize(Policy = "Harcama Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _expenseService.GetExpenseAsync(x => x.Id == id, x => x.AppUser, x => x.Extra);
            if (expense == null)
            {
                return NotFound();
            }

            var model = new ExpenseViewModel()
            {
                Id = expense.Id,
                Name = expense.Name,
                Date = expense.Date,
                ExtraId = expense.ExtraId,
                UserName = $"{expense.AppUser.FirstName} {expense.AppUser.LastName}",
                Amount = expense.Amount,
                PriceEach = expense.PriceEach
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            _expenseService.DeleteExpenseFromDatabase(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Harcama silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Harcama silindi";
            return RedirectToAction(nameof(Index));
        }

    }
}
