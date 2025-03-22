using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles = "admin,agencyManager,busManager,carManager,hotelManager")]
    public class AdminDashboardController : Controller
    {
        private readonly IAdminUserService _userService;
        private readonly ISessionService _sessionService;
        public AdminDashboardController(IAdminUserService userService, ISessionService sessionService)
        {
            _userService = userService;
            _sessionService = sessionService;
        }

        [Authorize(Roles ="admin,agencyManager,busManager,carManager,hotelManager")]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Admin/AdminDashboard/index");
            return View();
        }


      
    }
}
