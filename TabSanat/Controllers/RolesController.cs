using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabSanat.Helpers;
using TabSanat.Model;
using TabSanat.Services.Interfaces;
using TabSanat.ViewModels.Identity;

namespace TabSanat.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ISaveService _save;

        public RolesController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ISaveService save)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _save = save;
        }


        public async Task<IActionResult> Index()
        {
            var roles = _roleManager.Roles;

            return await Task.Run(() => View(roles));
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: PaymentTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                bool x = await _roleManager.RoleExistsAsync(role.Name);

                if (!x)
                {
                    // first we create Admin rool    
                    var newRole = new IdentityRole();
                    newRole.Name = role.Name;
                    await _roleManager.CreateAsync(newRole);


                }

                return RedirectToAction(nameof(Index));
            }
            TempData["SMessage"] = "Kayıt Başarısız";
            return View();
        }

        // GET: PaymentTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var existingRoleClaims = await _roleManager.GetClaimsAsync(role);

            var model = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };

            foreach (var claim in ClaimData.Claims)
            {
                RoleClaim roleClaim = new RoleClaim()
                {
                    ClaimType = claim
                };

                if (existingRoleClaims.Any(c => c.Value == claim))
                {
                    roleClaim.IsSelected = true;
                }
                model.Claims.Add(roleClaim);
            }
            model.Claims.OrderBy(x => x.ClaimType);

            return View(model);
        }

        // POST: PaymentTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (id != model.Id)
                return NotFound();


            if (ModelState.IsValid)
            {
                var result = new IdentityResult();
                var oldRole = await _roleManager.FindByIdAsync(id);
                oldRole.Name = model.Name;

                result = await _roleManager.UpdateAsync(oldRole);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "İsim değiştirilemedi.");
                    return View(model);
                }

                var claims = await _roleManager.GetClaimsAsync(oldRole);
                foreach (var claim in claims)
                {
                    result = await _roleManager.RemoveClaimAsync(oldRole, claim);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Yetki kaldırılamadı.");
                        return View(model);
                    }
                }

                foreach (var claim in model.Claims.Where(x => x.IsSelected == true))
                {
                    result = await _roleManager.AddClaimAsync(oldRole, new Claim(claim.ClaimType, claim.ClaimType));
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Yetki eklenemedi.");
                        return View(model);
                    }
                }

                return RedirectToAction("Edit", new { id = model.Id });
            }
            return View(model);
        }

        // GET: PaymentTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();


            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            await _roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }

    }
}
