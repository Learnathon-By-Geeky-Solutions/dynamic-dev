using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles = "admin,agencyManager,busManager,carManager,hotelManager")]
    public class AdminDashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ISessionService _sessionService;
        public AdminDashboardController(IUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [Authorize(Roles ="admin,agencyManager,busManager,carManager,hotelManager")]
        public IActionResult Index()
        {
            return View();
        }


      
    }
}
