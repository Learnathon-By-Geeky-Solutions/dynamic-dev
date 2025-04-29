using EasyTravel.Application.Services;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,hotelManager")]
    public class AdminHotelBookingController : Controller
    {
        private readonly IAdminHotelBookingService _adminHotelBookingService;
        public AdminHotelBookingController(IAdminHotelBookingService adminHotelBookingService)
        {
            _adminHotelBookingService = adminHotelBookingService;
        }
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hotelBookings = await _adminHotelBookingService.GetPaginatedHotelBookingsAsync(pageNumber, pageSize);
            return View(hotelBookings);
        }
    }
}
