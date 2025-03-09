using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyTravel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect(returnUrl ?? Url.Content("~/"));
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        if (roles.Contains("busmanager"))
                        {
                            return RedirectToAction("Index", "AdminBus", new { area = "Admin" });
                        }
                        if (roles.Contains("admin"))
                        {
                            return RedirectToAction("Index", "AdminDashboard", new { area = "Admin" });
                        }
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        return RedirectToAction("Index", "Home", new { area = string.Empty });
                    }
                    ModelState.AddModelError(string.Empty, "Invalid login attempt!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (_signInManager.IsSignedIn(User))
            {
                return Redirect(returnUrl ?? Url.Content("~/"));
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            if (ModelState.IsValid)
            {
                var user = new User { FirstName = model.FirstName, LastName = model.LastName, DateOfBirth = model.DateOfBirth, Gender = model.Gender, Email = model.Email, UserName = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "client");
                    return RedirectToAction("Login", "Account", new { area = string.Empty });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = string.Empty });
        }
    }
}
