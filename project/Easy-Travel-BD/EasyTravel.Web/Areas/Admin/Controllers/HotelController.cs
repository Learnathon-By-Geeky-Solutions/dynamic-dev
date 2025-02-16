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
            //var hotels = _hotelService.GetAll();
            //return View(hotels);
            return View();
        }

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
    }
}
