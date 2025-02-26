using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService, IHotelService hotelService)
        {
            _roomService = roomService;
            _hotelService = hotelService;
        }
        public IActionResult Index()
        {
            var rooms = _roomService.GetAll();
            return View(rooms);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Hotels = new SelectList(_hotelService.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Room model)
        {
            if (ModelState.IsValid)
            {
                _roomService.Create(model);
                return RedirectToAction("Index", "Room", new { area = "Admin" });
            }
            // Log validation errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    // You can log the error message or inspect it in the debugger
                    Console.WriteLine($"Property: {state.Key}, Error: {error.ErrorMessage}");
                }
            }
            ViewBag.Hotels = new SelectList(_hotelService.GetAll(), "Id", "Name");
            return View(model);
          //  return View();
        }
    }
}
