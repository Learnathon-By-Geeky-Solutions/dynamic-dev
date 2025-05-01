using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin,hotelManager")]
    public class AdminHotelController : Controller
    {
        private readonly IHotelService _hotelService;
        public AdminHotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hotels = _hotelService.GetAllPaginatedHotels(pageNumber, pageSize);
            return View(hotels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Hotel model)
        {
            if (ModelState.IsValid)
            {
                TempData["success"] = "The hotel has been created successfully";
                _hotelService.Create(model);
                return RedirectToAction("Index", "AdminHotel", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if (ModelState.IsValid)
            {
                var hotel = _hotelService.Get(id);
                return View(hotel);
            }
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Hotel model)
        {
            if (ModelState.IsValid)
            {
                _hotelService.Update(model);
                TempData["success"] = "The hotel has been updated successfully";

                return RedirectToAction("Index", "AdminHotel", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hotel = _hotelService.Get(id);
            return View(hotel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Hotel model)
        {
            if (ModelState.IsValid)
            {
                _hotelService.Delete(model.Id);
                return RedirectToAction("Index", "AdminHotel", new { area = "Admin" });
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult HotelDetails(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id == Guid.Empty)
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            var hotel = _hotelService.Get(id);
            return View(hotel);

        }
    }
}
