using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin,hotelManager")]
    public class AdminRoomController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        public AdminRoomController(IRoomService roomService, IHotelService hotelService)
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
        [HttpPost,ValidateAntiForgeryToken]
        public IActionResult Create(Room model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "The room has been created successfully";
                _roomService.Create(model);
                return RedirectToAction("Index", "AdminRoom", new { area = "Admin" });
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
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (id == Guid.Empty)
                    return RedirectToAction("Error", "Home", new { area = "Admin" });
                var room = _roomService.Get(id);
                ViewBag.Hotels = new SelectList(_hotelService.GetAll(), "Id", "Name");

                return View(room);
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Room model)
        {
            if (ModelState.IsValid)
            {
                _roomService.Update(model);
                TempData["success"] = "The room has been updated successfully";

                return RedirectToAction("Index", "AdminRoom", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (id == Guid.Empty)
                {
                    return RedirectToAction("Error", "Home", new { area = "Admin" });
                }
                var room = _roomService.Get(id);
                return View(room);
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Room model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id == Guid.Empty)
                {
                    return RedirectToAction("Error", "Home", new { area = "Admin" });
                }
                _roomService.Delete(model.Id);
                TempData["success"] = "The room has been delete successfully";

                return RedirectToAction("Index", "AdminRoom", new { area = "Admin" });
            }
            return View(model);
        }

    }
}
