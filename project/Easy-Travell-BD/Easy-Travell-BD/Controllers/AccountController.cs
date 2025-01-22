using EasyTravel.Application.Services.Interfaces;
using EasyTravel.Domain.Models;
using Microsoft.AspNetCore.Mvc;



namespace Easy_Travell_BD.Controllers
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

            if (_userService.AuthenticateUser(email, password)){
                var user = _userService.GetUserByEmail(email);
                return user.Role == "Admin" ? RedirectToAction("Create", "Bus") : RedirectToAction("List", "Bus");

            }

            return View();

        }

        public IActionResult Register() => View();


        [HttpPost]
        public IActionResult Register(User user)
        {
            _userService.RegisterUser(user);
            return RedirectToAction("Login");
        }










        
    }
}
