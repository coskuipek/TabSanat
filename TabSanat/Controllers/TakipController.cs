using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;

namespace TabSanat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TakipController : Controller
    {
        private readonly IHistoryService _historyService;
        private readonly ISaveService _save;

        public TakipController(IHistoryService historyService, ISaveService save)
        {
            _historyService = historyService;
            _save = save;
        }

        // GET: Takip
        public async Task<IActionResult> Index()
        {

            var model = new List<HistoryViewModel>();

            var histories = await _historyService.GetAllAsync(null, null, x => x.AppUser);

            foreach (var history in histories)
            {
                var modelItem = new HistoryViewModel()
                {
                    Id = history.Id,
                    UserId = history.AppUserId,
                    UserName = $"{history.AppUser.FullName}",
                    DateTime = history.DateTime,
                    Description = history.Description
                };
                model.Add(modelItem);

            }

            return View(model);
        }

        // GET: Takip/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var history = await _historyService.GetHistoryAsync(x => x.Id == id, x => x.AppUser);
            if (history == null)
            {
                return NotFound();
            }
            var model = new HistoryViewModel()
            {
                Id = history.Id,
                UserId = history.AppUserId,
                UserName = $"{history.AppUser.FullName}",
                DateTime = history.DateTime,
                Description = history.Description
            };

            return View(model);
        }

        // POST: Takip/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _historyService.DeleteHistory(id);
            await _save.Completeasync();
            return RedirectToAction(nameof(Index));
        }


    }
}
