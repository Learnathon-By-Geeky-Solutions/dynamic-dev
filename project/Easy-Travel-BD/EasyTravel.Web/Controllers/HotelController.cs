using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using EasyTravel.Domain.ValueObjects;

namespace EasyTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IHotelBookingService _hotelBookingService;
        public HotelController(IHotelService hotelService,IRoomService roomService , IHotelBookingService hotelBooking)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _hotelBookingService = hotelBooking;
        }

        public IActionResult Index(string location, DateTime? travelDateTime,int pageNumber = 1,int pageSize =10)
        {
            PagedResult<Hotel> hotels;

            if (!string.IsNullOrEmpty(location))
            {
                hotels = _hotelService.SearchHotels(location, travelDateTime, pageNumber, pageSize);
            }
            else
            {
                hotels = _hotelService.GetAllPaginatedHotels(pageNumber,pageSize);
            }

            ViewBag.Location = location;
            ViewBag.TravelDateTime = travelDateTime;

            return View(hotels);
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

        [HttpGet]
        public IActionResult HotelBooking(Guid hotelId, Guid roomId)
        {
            var hotel = _hotelService.Get(hotelId);
            var room = _roomService.Get(roomId);


            HotelBookingViewModel viewModel = new HotelBookingViewModel
            {
                hotel = hotel,
                room = room,               
            };
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult HotelBookingRoomDetails(Guid hotelId, Guid roomId)
        {
            var hotel = _hotelService.Get(hotelId);
            var room = _roomService.Get(roomId);


            HotelBookingViewModel viewModel = new HotelBookingViewModel
            {
                hotel = hotel,
                room = room,
                // hotelBooking = { }
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HotelBooking(HotelBookingViewModel viewModel)
        {
            if (viewModel == null || viewModel.hotelBooking == null)
            {
                TempData["error"] = "Hotel Booking Failed! Invalid Data";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Validation Failed!";
                return View(viewModel);
            }

            try
            {
                _hotelBookingService.Create(viewModel.hotelBooking);
                TempData["success"] = "Hotel booked successfully";
                return RedirectToAction("SuccessPage");
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred: " + ex.Message;
            }

            return View(viewModel);
        }

        public IActionResult HotelRoom(Guid id)
        {
            var rooms = _roomService.GetRoomByHotel(id);

            return View(rooms);

        }
        
    }
}
