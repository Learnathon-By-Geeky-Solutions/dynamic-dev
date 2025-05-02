using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using EasyTravel.Domain.ValueObjects;
using EasyTravel.Domain.Enums;
using System.Security.Claims;
using EasyTravel.Web.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EasyTravel.Web.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IRoomService _roomService;
        private readonly IHotelBookingService _hotelBookingService;
        private readonly IBookingService _bookingService;
        public HotelController(IHotelService hotelService,IRoomService roomService , IHotelBookingService hotelBooking,IBookingService booking)
        {
            _hotelService = hotelService;
            _roomService = roomService;
            _hotelBookingService = hotelBooking;
            _bookingService = booking;
        }
        [HttpGet]
        public IActionResult Index(string? location, DateTime? travelDateTime,int pageNumber = 1,int pageSize =10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
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
        private Booking GetTemporaryBooking()
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                TotalAmount = 0,
                BookingStatus = BookingStatus.Pending,
                BookingTypes = BookingTypes.Hotel,
                UserId = User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!) : Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            return booking;
        }
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hotel = _hotelService.Get(id);
            if (hotel == null)
            {
                return NotFound();
            }
            var rooms = await _roomService.GetRoomByHotel(id) ?? new List<Domain.Entites.Room>();

            var viewModel = new HotelRoomViewModel
            {
                Hotel = hotel,
                Rooms = rooms.ToList(),
            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult HotelBookingRoomDetails(Guid hotelId, Guid roomId)
        { 
            if(!ModelState.IsValid)
            {
                return View();
            }
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
        public IActionResult HotelBooking(Guid hotelId, Guid roomId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var hotel = _hotelService.Get(hotelId);
            var room = _roomService.Get(roomId);

            var tempBooking = GetTemporaryBooking();
            _bookingService.AddBooking(tempBooking);

            var viewModel = new BookingModel
            {
                Id = tempBooking.Id,
                TotalAmount = room.PricePerNight,
                HotelBooking = new HotelBooking
                {
                    Id = tempBooking.Id,
                    HotelId = hotelId,
                    RoomIdsJson = roomId.ToString(),
                    CheckInDate = DateTime.UtcNow,
                    CheckOutDate = DateTime.UtcNow,
                },
            };
            viewModel.HotelBooking.Hotel = hotel;
            viewModel.HotelBooking.Hotel.Room = room;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult HotelBooking(BookingModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Validation Failed!";
                return View(viewModel);
            }

            try
            {
                var hotelBooking = viewModel.HotelBooking;
                var booking = _bookingService.Get(viewModel.Id);
                booking.TotalAmount = viewModel.TotalAmount;
                _hotelBookingService.SaveBooking(hotelBooking!, booking);
                TempData["success"] = "Hotel booked successfully";
                return RedirectToAction("Pay", "Payment", new {id1 = Guid.NewGuid()});
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred: " + ex.Message;
            }

            return View(viewModel);
        }

        public IActionResult HotelRoom(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var rooms = _roomService.GetRoomByHotel(id);

            return View(rooms);

        }
        
    }
}
