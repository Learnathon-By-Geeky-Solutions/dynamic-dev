
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;



namespace EasyTravel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        public AccountController(IUserService userService, ISessionService sessionService)
        {

            _userService = userService;
            _sessionService = sessionService;
        }

        public IActionResult Login()
        {
            var isLoggedIn = _userService.IsLoggedIn(_sessionService.GetString("UserLoggedIn"));
            return isLoggedIn == false ? View() : RedirectToAction("Error","Home",new { area = string.Empty});
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
                    ? RedirectToAction("Index", "Dashboard", new { area = "Admin" })
                    : RedirectToAction("Index", "Home", new {area = string.Empty});
            }
            ViewBag.ErrorMessage = "Invalid email or password.";
            return View();

        }

        public IActionResult Register()
        {
            var isLoggedIn = _userService.IsLoggedIn(_sessionService.GetString("UserLoggedIn"));
            return isLoggedIn == false ? View() : RedirectToAction("Error", "Home", new { area = string.Empty });
        }


        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {

                _userService.RegisterUser(user);
                return RedirectToAction("Login");
            }

            return View();
           
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
