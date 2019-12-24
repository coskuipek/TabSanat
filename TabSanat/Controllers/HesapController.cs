using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabSanat.Helpers;
using TabSanat.Model;
using TabSanat.ViewModels.Identity;

namespace TabSanat.Controllers
{
    public class HesapController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HesapController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        #region SEED ROLES
        private async Task CreateRolesandUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var adminRole = new IdentityRole();
                adminRole.Name = "Admin";
                await _roleManager.CreateAsync(adminRole);

                foreach (var item in ClaimData.Claims)
                    await _roleManager.AddClaimAsync(adminRole, new Claim(item, item));


                //Here we create a Admin super user who will maintain the website                   

                var user = new AppUser
                {
                    UserName = "cosku",
                    FirstName = "Coşku",
                    LastName = "İpek"
                };


                string userPWD = "cosku";

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

        public async Task<IActionResult> Seed()
        {
            await CreateRolesandUsers();
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login(string returnURL)
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(string returnURL, LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(returnURL);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Hatalı Giriş");
            return View(model);
        }
        #endregion

        #region Logout
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Login");
        }
        #endregion

        #region Index
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var model = new List<AccountViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var singleRole = roles.First();
                var modelItem = new AccountViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleName = singleRole
                };
                model.Add(modelItem);
            }

            return View(model);
        }
        #endregion

        #region Create User
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.AllRoles = new SelectList(_roleManager.Roles, "Id", "Name");
            return await Task.Run(() => View());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(AccountDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    var roleName = await _roleManager.GetRoleNameAsync(role);
                    var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (roleResult.Succeeded)
                        return RedirectToAction(nameof(Index));
                    ModelState.AddModelError("", "Bir hata oluştu.");
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "Bir hata oluştu..");
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Bir hata oluştu...");
            return View(model);
        }
        #endregion

        #region Edit User
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            var role = await _userManager.GetRolesAsync(user);
            var existingRole = role.First();
            if (existingRole != null)
            {
                string existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                AccountEditViewModel model = new AccountEditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    RoleId = existingRoleId
                };
                ViewBag.AllRoles = new SelectList(_roleManager.Roles, "Id", "Name", model);
                return View(model);
            }
            return View();


        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(string Id, AccountEditViewModel model)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                var oldRole = await _userManager.GetRolesAsync(user);
                var stringOldRole = oldRole.First();
                var newRole = await _roleManager.FindByIdAsync(model.RoleId);

                if (stringOldRole != newRole.Name)
                {
                    await _userManager.RemoveFromRoleAsync(user, stringOldRole);
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                }
                if (model.NewPasswordFirst != null && model.NewPasswordSecond != null)
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _userManager.AddPasswordAsync(user, model.NewPasswordSecond);
                }
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }

            return View(model);

        }
        #endregion

        #region Access Denied
        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion


    }
}