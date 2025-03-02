using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        public HotelController(IHotelService hotelService,IRoomService roomService)
        {
            _hotelService = hotelService;
            _roomService = roomService;
        }

        public IActionResult Index()
        {
            var hotels = _hotelService.GetAll();
            return View(hotels);
            //return View();
        }
        [HttpGet]
        public IActionResult Details(Guid id)
        {

            var hotel = _hotelService.Get(id);
            if (hotel == null)
            {
                return NotFound();
            }
            var rooms = _roomService.GetRoomByHotel(id) ?? new List<Domain.Entites.Room>();

            var viewModel = new HotelRoomViewModel
            {
                Hotel = hotel,
                Rooms = (List<Domain.Entites.Room>)rooms
            };

            return View(viewModel);
        }


        public IActionResult HotelRoom(Guid id)
        {
            var rooms = _roomService.GetRoomByHotel(id);

            return View(rooms);

        }
        
    }
}
