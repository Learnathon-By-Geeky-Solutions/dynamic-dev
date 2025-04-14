using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Guide()
        {
            var list = await _bookingHistoryService.GetGuideBookingsAsync(GetId());
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Photographer()
        {
            var list = await _bookingHistoryService.GetPhotographerBookingsAsync(GetId());
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Bus()
        {
            var list = await _bookingHistoryService.GetBusBookingsAsync(GetId());
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Car()
        {
            var list = await _bookingHistoryService.GetCarBookingsAsync(GetId());
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Hotel()
        {
            var list = await _bookingHistoryService.GetHotelBookingsAsync(GetId());
            return View(list);
        }
        private Guid GetId()
        {
            return GetId();
        }
    }
}
