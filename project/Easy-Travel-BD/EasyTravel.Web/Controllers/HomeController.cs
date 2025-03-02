using System.Diagnostics;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public IActionResult Index()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            return View();
        }

        public IActionResult Privacy()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
