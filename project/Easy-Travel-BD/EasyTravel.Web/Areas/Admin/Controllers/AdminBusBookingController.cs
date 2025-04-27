using EasyTravel.Domain.Entites;
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
        [HttpGet]
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var list = _adminBusBookingService.GetPaginatedBusBookingsAsync(pageNumber, pageSize);
            return View(list);
        }
        [HttpPost]
        public IActionResult Delete (Guid Id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _adminBusBookingService.DeleteBusBooking(Id);

            return RedirectToAction("Index", "AdminBusBooking");
        }
    }
}
