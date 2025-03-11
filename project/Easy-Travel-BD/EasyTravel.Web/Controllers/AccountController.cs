using Microsoft.AspNetCore.Http.Extensions;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using EasyTravel.Application.Services;
using System.Threading.Tasks;

namespace EasyTravel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IAuthService _authService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var refererUrl = HttpContext.Session.GetString("LastVisitedPage");
                return Redirect(string.IsNullOrEmpty(refererUrl) ? "/Home/Index" : refererUrl);
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (success, errorMessage, redirectUrl) = await _authService.AuthenticateUserAsync(model.Email, model.Password, false);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }
            if (string.IsNullOrEmpty(redirectUrl))
            {
                var refererUrl = HttpContext.Session.GetString("LastVisitedPage");
                return Redirect(string.IsNullOrEmpty(refererUrl) ? "/Home/Index" : refererUrl);
            }
            return Redirect(redirectUrl);
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var refererUrl = HttpContext.Session.GetString("LastVisitedPage");
                return Redirect(string.IsNullOrEmpty(refererUrl) ? "/Home/Index" : refererUrl);
            }
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var (success, errorMessage) = await _authService.RegisterUserAsync(model.FirstName, model.LastName, model.DateOfBirth, model.Gender, model.Email, model.Email, model.Password);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();
            string? refererUrl = HttpContext.Session.GetString("LastVisitedPage");
            if(!string.IsNullOrEmpty(refererUrl))
            {
                if(refererUrl.Contains("Admin"))
                {
                    HttpContext.Session.Remove("LastVisitedPage");
                    refererUrl = "/Home/Index";
                }
                return Redirect(refererUrl);

            }
            return Redirect("/Home/Index");
        }
    }
}
