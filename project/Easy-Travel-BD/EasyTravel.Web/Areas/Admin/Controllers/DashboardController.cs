using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var isLoggedIn = _userService.IsLoggedIn(_sessionService.GetString("UserLoggedIn"));
            var isAdmin = _userService.IsAdmin(_sessionService.GetString("UserRole"));
            return isLoggedIn == true && isAdmin == true ? View() : RedirectToAction("Error","Home",new {area = string.Empty});
        }
    }
}
