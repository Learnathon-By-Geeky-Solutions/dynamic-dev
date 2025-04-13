using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,agencyManager")]
    public class AdminHotelBookingController : Controller
    {
        private readonly IAdminHotelBookingService _adminHotelBookingService;
        public AdminHotelBookingController(IAdminHotelBookingService adminHotelBookingService)
        {
            _adminHotelBookingService = adminHotelBookingService;
        }
        public IActionResult Index()
        {
            var list = _adminHotelBookingService.GetAll();
            return View(list);
        }
    }
}
