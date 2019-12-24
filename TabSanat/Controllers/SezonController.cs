using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class SezonController : Controller
    {

        private readonly ISeasonService _seasonService;
        private readonly ICourseService _courseService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISaveService _save;

        public SezonController(ISeasonService seasonService, ICourseService courseService, UserManager<AppUser> userManager, ISaveService save)
        {
            _seasonService = seasonService;
            _courseService = courseService;
            _userManager = userManager;
            _save = save;
        }

        [Authorize(Policy = "Sezon Liste")]
        public async Task<IActionResult> Index()
        {
            var seasons = await _seasonService.GetAllAsync(x => x.IsDeleted == false, x => x.OrderByDescending(y => y.EndDate));

            var model = new SeasonMaps().SeasonIndexMap(seasons);

            return View(model);
        }

        [Authorize(Policy = "Sezon Detay")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var season = await _seasonService.GetSeasonAsync(m => m.Id == id);
            if (season == null)
            {
                return NotFound();
            }
            var model = new SeasonViewModel()
            {
                Id = season.Id,
                Name = season.Name,
                StartDate = season.StartDate,
                EndDate = season.EndDate,
                AmountOfMonths = season.MonthsInSeason
            };

            var coursesInSeason = await _courseService.GetAllAsync(x => x.SeasonId == season.Id);

            model.CoursesInSeason = new List<CourseViewModel>();
            foreach (var course in coursesInSeason)
            {
                CourseViewModel courseView = new CourseViewModel
                {
                    Id = course.Id,
                    Name = course.Name,
                    OneLessonPrice = _courseService.CourseTotalPrice(course.Id)
                };
                model.CoursesInSeason.Add(courseView);
            }

            return View(model);
        }

        [Authorize(Policy = "Sezon Ekle")]
        public IActionResult Create()
        {
            var model = new SeasonFormModel
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeasonFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                Season season = new Season()
                {
                    Name = _save.FixName(model.Name.ToLower()),
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    AppUserId = user.Id
                };

                _seasonService.AddSeason(season);
                var changes = await _save.Completeasync("Yeni sezon oluşturuldu", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Yeni sezon oluşturuldu";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Sezon Değiştir")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var season = await _seasonService.GetSeasonAsync(x => x.Id == id);
            if (season == null)
            {
                return NotFound();
            }
            SeasonFormModel model = new SeasonFormModel()
            {
                Id = season.Id,
                Name = season.Name,
                StartDate = season.StartDate,
                EndDate = season.EndDate
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, SeasonViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                var season = await _seasonService.GetSeasonAsync(x => x.Id == id);

                season.Name = _save.FixName(model.Name);
                season.StartDate = model.StartDate;
                season.EndDate = model.EndDate;

                var changes = await _save.Completeasync("Sezon düzenlendi", user);
                if (changes > 0)
                    TempData["SMessage"] = $"Sezon düzenlendi";


                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Policy = "Sezon Sil")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }

            var season = await _seasonService.GetSeasonAsync(m => m.Id == id);

            if (season == null)
            {
                return NotFound();
            }
            SeasonViewModel model = new SeasonViewModel()
            {
                Id = season.Id,
                Name = season.Name
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var season = await _seasonService.GetSeasonAsync(m => m.Id == id);
            _seasonService.FakeDeleteSeason(season);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Sezon silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Sezon silindi";
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> RealDelete(Guid id)
        {
            _seasonService.DeleteSeason(id);
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Sezon tamamen silindi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Sezon tamamen silindi";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnDelete(Guid id)
        {
            var season = await _seasonService.GetSeasonAsync(m => m.Id == id);

            season.IsDeleted = false;

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var changes = await _save.Completeasync("Sezon geri yüklendi", user);
            if (changes > 0)
                TempData["SMessage"] = $"Sezon geri yüklendi";
            return RedirectToAction(nameof(Index));
        }

    }
}
