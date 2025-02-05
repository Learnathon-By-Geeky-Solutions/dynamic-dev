using System.Diagnostics;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;
        private readonly IUserService _userService;
        public HomeController(ILogger<HomeController> logger,ISessionService sessionService, IUserService userService)
        {
            _logger = logger;
            _sessionService = sessionService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            HttpContext.Response.Headers["Cache-Control"]= "no-store, no-cache, must-revalidate";
            var isLoggedIn = _userService.IsLoggedIn(_sessionService.GetString("UserLoggedIn"));
            var controller = _userService.GetUserController(_sessionService.GetString("UserRole"));
            return isLoggedIn == false ? View() : RedirectToAction("Index", controller, new {area = "Admin"});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
