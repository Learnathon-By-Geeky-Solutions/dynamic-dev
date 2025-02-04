
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;



namespace EasyTravel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {

            _userService = userService;
        }

        public IActionResult Login()
        {

            return View();
        }



        [HttpPost]
        public IActionResult Login(string email, string password)
        {

            if (_userService.AuthenticateUser(email, password))
            {
                var user = _userService.GetUserByEmail(email);

                HttpContext.Session.SetString("UserLoggedIn", "true");
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserName", user.Name);

                return user.Role == "Admin"
                    ? RedirectToAction("Create", "Bus")
                    : RedirectToAction("List", "Bus");
            }
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();

        }

        public IActionResult Register() => View();


        [HttpPost]
        public IActionResult Register(User user)
        {
            _userService.RegisterUser(user);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }






    }
}
