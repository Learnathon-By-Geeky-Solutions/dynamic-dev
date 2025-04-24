using System.Diagnostics;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EasyTravel.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ISessionService _sessionService;
        private readonly IAdminUserService _userService;
        public HomeController(ILogger<HomeController> logger,ISessionService sessionService, IAdminUserService userService,SignInManager<User> signInManager)
        {
            _logger = logger;
            _sessionService = sessionService;
            _userService = userService;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage","/Home/Index");
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> RecommendedPhotographers()
        {
            using var client = new HttpClient();
            var response = await client.GetStringAsync("https://localhost:{port}/api/recommendation/photographers?count=3");
            var photographers = JsonConvert.DeserializeObject<List<Photographer>>(response);

            return PartialView("_RecommendedPhotographers", photographers);
        }

    }
}
