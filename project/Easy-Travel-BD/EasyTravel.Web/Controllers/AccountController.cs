
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;



namespace EasyTravel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            { 
                var result = await _signInManager.PasswordSignInAsync(model.Email!,model.Password!, model.RememberMe,false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = string.Empty });
                }
                ModelState.AddModelError(string.Empty, "Invalid Login attemp !");
            }
            return View(model);

        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { FirstName = model.FirstName, LastName = model.LastName, DateOfBirth = model.DateOfBirth, Gender = model.Gender, Email = model.Email,UserName = model.Email };
                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account", new {area = string.Empty});
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
           
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = string.Empty });
        }
    }
}
