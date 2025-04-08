using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Profile.Controllers
{
    [Area("Profile"),Authorize]
    public class BookingHistoryController : Controller
    {
        private readonly IBookingHistoryService _bookingHistoryService;
        public BookingHistoryController(IBookingHistoryService bookingHistoryService)
        {
            _bookingHistoryService = bookingHistoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Guide()
        {
            return View();
        }
    }
}
