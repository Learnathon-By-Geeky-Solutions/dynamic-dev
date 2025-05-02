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
        public async Task<IActionResult> Guide(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _bookingHistoryService.GetGuideBookingsAsync(GetId(), pageNumber, pageSize);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Photographer(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _bookingHistoryService.GetPhotographerBookingsAsync(GetId(), pageNumber, pageSize);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Bus(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _bookingHistoryService.GetBusBookingsAsync(GetId(), pageNumber, pageSize);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Car(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _bookingHistoryService.GetCarBookingsAsync(GetId(), pageNumber, pageSize);
            return View(list);
        }
        [HttpGet]
        public async Task<IActionResult> Hotel(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var list = await _bookingHistoryService.GetHotelBookingsAsync(GetId(),pageNumber,pageSize);
            return View(list);
        }
        private Guid GetId()
        {
            return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
