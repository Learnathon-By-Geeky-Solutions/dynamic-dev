using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,busManager")]
    public class AdminBusBookingController : Controller
    {
        private readonly IAdminBusBookingService _adminBusBookingService;
        public AdminBusBookingController(IAdminBusBookingService adminBusBookingService)
        {
            _adminBusBookingService = adminBusBookingService;
        }
        public IActionResult Index()
        {
            var list = _adminBusBookingService.GetAllBusBookings();
            return View(list);
        }
    }
}
