using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public IActionResult Index()
        {
            var hotels = _hotelService.GetAll();
            return View(hotels);
            //return View();
        }
       /* public IActionResult HotelWithroom()
        {
            var hotels = _hotelService.GetAllHotelsWithRooms(); // Fetch hotels with rooms
            return View(hotels);
        }*/


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Hotel model)
        {
            if (ModelState.IsValid)
            {
                _hotelService.Create(model);
                return RedirectToAction("Index", "Hotel", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            if(id==Guid.Empty)
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            var hotel = _hotelService.Get(id);
            return View(hotel);
        }
        [HttpPost]
        public IActionResult Update(Hotel model)
        {
            if (ModelState.IsValid)
            {
                _hotelService.Update(model);
                return RedirectToAction("Index", "Hotel", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var hotel = _hotelService.Get(id);
            return View(hotel);
        }
        [HttpPost]
        public IActionResult Delete(Hotel model)
        {
            if (model.Id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _hotelService.Delete(model.Id);
            return RedirectToAction("Index", "Hotel", new { area = "Admin" });
        }

        public IActionResult HotelDetails(Guid id)
        {
            if (id == Guid.Empty)
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            var hotel = _hotelService.Get(id);
            return View(hotel);

        }
    }
}
