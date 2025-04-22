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
        public BookingController(IMapper mapper,ISessionService sessionService,IPhotographerBookingService photographerBookingService, IGuideBookingService guideBookingService,IBusService busService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _photographerBookingService = photographerBookingService;
            _guideBookingService = guideBookingService;
            _busService = busService;
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
            return RedirectToAction("PhotographerBooking", "Review");
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
                    TotalAmount = model.TotalAmount,
                    BookingTypes = BookingTypes.Photographer,
                    BookingStatus = BookingStatus.Pending,
                };
                if (User?.Identity?.IsAuthenticated == true)
                {
                    booking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                pgBooking.PhotographerId = Guid.Parse(_sessionService.GetString("PhotographerId"));
                _sessionService.Remove("PhotographerId");
                _photographerBookingService.AddBooking(pgBooking,booking);
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
            return RedirectToAction("GuideBooking", "Review");
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
                    TotalAmount = model.TotalAmount,
                    BookingTypes = BookingTypes.Guide,
                    BookingStatus = BookingStatus.Pending
                };
                if (User.Identity.IsAuthenticated == true)
                {
                    booking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                guideBooking.GuideId = Guid.Parse(_sessionService.GetString("GuideId"));
                _guideBookingService.AddBooking(guideBooking,booking);
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
            var busbooking = new BusBooking
            {
                Id = Guid.NewGuid(),
                BusId = model.BusId,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                //TotalAmount = model.TotalAmount, // Using model.TotalAmount directly instead of BookingForm.TotalAmount
                BookingDate = DateTime.Now,
                SelectedSeats = model.SelectedSeatNumbers,
                SelectedSeatIds = model.SelectedSeatIds,
            };

            // Adding UserId to the BusBooking if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                //busbooking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            _busService.SaveBooking(busbooking, model.SelectedSeatIds);
            return View("Success");
        }
    }
}
