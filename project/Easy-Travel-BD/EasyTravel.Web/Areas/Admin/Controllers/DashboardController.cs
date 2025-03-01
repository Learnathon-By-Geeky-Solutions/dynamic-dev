using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles = "admin")]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        public DashboardController(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            return View();
        }
    }
}
