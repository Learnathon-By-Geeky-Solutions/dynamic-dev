using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
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
        [HttpGet]
        public IActionResult  Details(Guid id)
        {
            var hotel = _hotelService.Get(id);
            return View(hotel);
            //return View();
        }

        
    }
}
