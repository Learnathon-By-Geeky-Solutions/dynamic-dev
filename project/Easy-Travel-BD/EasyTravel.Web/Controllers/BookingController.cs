using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
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
        public BookingController(IMapper mapper,ISessionService sessionService,IPhotographerBookingService photographerBookingService, IGuideBookingService guideBookingService,IBusService busService,ICarService carService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _photographerBookingService = photographerBookingService;
            _guideBookingService = guideBookingService;
            _busService = busService;
            _carService = carService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Photographer(Guid id)
        {
            _sessionService.SetString("PhotographerId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            return RedirectToAction("PhotographerBooking", "Review",id);
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Photographer(PhotographerBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pgBooking = _mapper.Map<PhotographerBooking>(model);
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    TotalAmount = model.TotalAmount,
                    BookingTypes = BookingTypes.Photographer,
                    BookingStatus = BookingStatus.Pending,
                };
                if (User?.Identity?.IsAuthenticated == true)
                {
                    booking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                pgBooking.Id = booking.Id;
                pgBooking.PhotographerId = model.PhotographerId;
                _sessionService.Remove("PhotographerId");
                _photographerBookingService.SaveBooking(pgBooking,booking);
                return View("Success");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Guide(Guid id)
        {
            _sessionService.SetString("GuideId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { area = string.Empty });
            }
            return RedirectToAction("GuideBooking", "Review",id);
        }
        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Guide(GuideBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guideBooking = _mapper.Map<GuideBooking>(model);
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    TotalAmount = model.TotalAmount,
                    BookingTypes = BookingTypes.Guide,
                    BookingStatus = BookingStatus.Pending
                };
                if (User.Identity.IsAuthenticated == true)
                {
                    booking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                guideBooking.Id = booking.Id;
                guideBooking.GuideId = model.GuideId;
                _guideBookingService.SaveBooking(guideBooking,booking);
                return View("Success");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Bus(Guid id)
        {
            _sessionService.SetString("BusId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            return RedirectToAction("SelectSeats", "Bus", id);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Bus(BusBookingViewModel model)
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
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                BookingDate = DateTime.Now,
                SelectedSeats = model.SelectedSeatNumbers,
                SelectedSeatIds = model.SelectedSeatIds,
            };
            _busService.SaveBooking(busbooking, model.SelectedSeatIds!,booking);
            return View("Success");
        }

        public IActionResult Car(Guid id)
        {
            _sessionService.SetString("BusId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            return RedirectToAction("PassengerDetails", "Car", new { id });
        }


        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Car(CarBookingViewModel model)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                TotalAmount = model.BookingForm.TotalAmount,
                BookingTypes = BookingTypes.Bus,
                BookingStatus = BookingStatus.Pending,
            };
            var carBooking = new CarBooking
            {
                Id = booking.Id,
                CarId = model.CarId,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                BookingDate = DateTime.Now,
            };
            _carService.SaveBooking(carBooking,model.CarId,booking);

            return View("Success");
        }
    }
}
