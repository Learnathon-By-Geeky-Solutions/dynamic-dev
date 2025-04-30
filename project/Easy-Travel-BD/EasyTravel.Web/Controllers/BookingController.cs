using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Enums;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace EasyTravel.Web.Controllers
{
    public class BookingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IPhotographerBookingService _photographerBookingService;
        private readonly IGuideBookingService _guideBookingService;
        private readonly IBusService _busService;
        private readonly ICarService _carService;
        private readonly IBookingService _bookingService;
        public BookingController(IMapper mapper, ISessionService sessionService, IPhotographerBookingService photographerBookingService, IGuideBookingService guideBookingService, IBusService busService, ICarService carService, IBookingService bookingService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _photographerBookingService = photographerBookingService;
            _guideBookingService = guideBookingService;
            _busService = busService;
            _carService = carService;
            _bookingService = bookingService;
        }
        private Booking GetTemporaryBooking()
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                TotalAmount = 0,
                BookingStatus = BookingStatus.Pending,
                UserId = User?.Identity?.IsAuthenticated == true ? Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!) : Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            return booking;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Photographer(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.Identity?.IsAuthenticated == false)
            {
                _sessionService.SetString("LastVisitedPage", "/Review/PhotographerBooking");
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            var tempBooking = GetTemporaryBooking();
            tempBooking.BookingTypes = BookingTypes.Photographer;
            _bookingService.AddBooking(tempBooking);
            return RedirectToAction("PhotographerBooking", "Review", new {id1 = tempBooking.Id,id2 = id});
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Photographer(BookingModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = _bookingService.Get(model.Id);
                _photographerBookingService.SaveBooking(model.PhotographerBooking!, booking);
                return View("Success");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Guide(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.Identity?.IsAuthenticated == false)
            {
                _sessionService.SetString("LastVisitedPage", "/Review/GuideBooking");
                return RedirectToAction("Login", "Account", new { area = string.Empty });
            }
            var tempBooking = GetTemporaryBooking();
            tempBooking.BookingTypes = BookingTypes.Guide;
            _bookingService.AddBooking(tempBooking);
            return RedirectToAction("GuideBooking", "Review", new { id1 = tempBooking.Id, id2 = id });
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Guide(BookingModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = _bookingService.Get(model.Id);
                _guideBookingService.SaveBooking(model.GuideBooking!, booking);
                return View("Success");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Bus(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _sessionService.SetString("BusId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                _sessionService.SetString("LastVisitedPage", "/Review/BusBooking");
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            var tempBooking = GetTemporaryBooking();
            tempBooking.BookingTypes = BookingTypes.Bus;
            _bookingService.AddBooking(tempBooking);
            return RedirectToAction("SelectSeats", "Bus", new { id1 = tempBooking.Id, id2 = id });
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Bus(BusBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    TotalAmount = model.TotalAmount,
                    BookingTypes = BookingTypes.Bus,
                    BookingStatus = BookingStatus.Pending,
                };
                var busbooking = new BusBooking
                {
                    Id = booking.Id,
                    BusId = model.BusId,
                    PassengerName = model.BookingForm?.PassengerName!,
                    Email = model.BookingForm?.Email!,
                    PhoneNumber = model.BookingForm?.PhoneNumber!,
                    BookingDate = DateTime.Now,
                    SelectedSeats = model.SelectedSeatNumbers,
                    SelectedSeatIds = model.SelectedSeatIds,
                };
                _busService.SaveBooking(busbooking, model.SelectedSeatIds!, booking);
                return View("Success");
            }
            return View(model);
        }

        public IActionResult Car(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (User.Identity?.IsAuthenticated == false)
            {
                _sessionService.SetString("LastVisitedPage", "/Review/Car");
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            var tempBooking = GetTemporaryBooking();
            tempBooking.BookingTypes = BookingTypes.Car;
            _bookingService.AddBooking(tempBooking);
            return RedirectToAction("PassengerDetails", "Car", new { id1 = tempBooking.Id, id2 = id });
        }


        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Car(CarBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    TotalAmount = model.BookingForm!.TotalAmount!,
                    BookingTypes = BookingTypes.Bus,
                    BookingStatus = BookingStatus.Pending,
                };
                var carBooking = new CarBooking
                {
                    Id = booking.Id,
                    CarId = model.CarId,
                    PassengerName = model.BookingForm.PassengerName!,
                    Email = model.BookingForm.Email!,
                    PhoneNumber = model.BookingForm.PhoneNumber!,
                    BookingDate = DateTime.Now,
                };
                _carService.SaveBooking(carBooking, model.CarId, booking);

                return View("Success");
            }
            return View();
        }
    }
}
