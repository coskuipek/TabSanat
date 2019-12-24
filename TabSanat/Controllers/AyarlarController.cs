using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Form;

namespace TabSanat.Controllers
{
    [Authorize]
    public class AyarlarController : Controller
    {
        private readonly IAppSettingsService _appSettings;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public AyarlarController(IAppSettingsService appSettings, UserManager<AppUser> userManager, ISaveService save)
        {
            _appSettings = appSettings;
            _userManager = userManager;
            _save = save;
        }

        public async Task<IActionResult> Index()
        {
            var model = new List<AppSettingsFormModel>();
            var settings = await _appSettings.GetAllAsync();

            foreach (var setting in settings)
            {
                var modelItem = new AppSettingsFormModel()
                {
                    Id = setting.Id,
                    SettingName = setting.SettingName,
                    Value = setting.Value
                };
                model.Add(modelItem);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<AppSettingsFormModel> models)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            foreach (var item in models)
            {
                var setting = await _appSettings.GetSettingAsync(x => x.Id == item.Id);
                setting.Value = item.Value;
            }
            var changes = await _save.Completeasync("Ayarlar düzenlendi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Ayarlar düzenlendi";
            return RedirectToAction("Index");
        }
    }
}