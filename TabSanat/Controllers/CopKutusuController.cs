using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Display;

namespace TabSanat.Controllers
{
    [Authorize]
    public class CopKutusuController : Controller
    {
        private readonly ISeasonService _seasonService;
        private readonly ISaveService _save;

        public CopKutusuController(ISeasonService seasonService, ISaveService save)
        {
            _seasonService = seasonService;
            _save = save;
        }
        [Authorize(Policy = "Çöp Kutusu")]
        public async Task<IActionResult> Index()
        {
            var seasons = await _seasonService.GetAllAsync(x => x.IsDeleted == true, x => x.OrderByDescending(y => y.EndDate));

            List<CopKutusuViewModel> listModel = new List<CopKutusuViewModel>();

            foreach (var season in seasons)
            {
                CopKutusuViewModel model = new CopKutusuViewModel()
                {
                    Id = season.Id,
                    Name = season.Name,
                    TypeName = "Sezon",
                    ControllerName = "Sezon"
                };
                listModel.Add(model);
            }

            return View(listModel);
        }
    }
}