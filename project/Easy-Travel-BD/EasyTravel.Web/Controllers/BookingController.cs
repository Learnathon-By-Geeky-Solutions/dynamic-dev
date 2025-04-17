using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
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
        public BookingController(IMapper mapper,ISessionService sessionService,IPhotographerBookingService photographerBookingService, IGuideBookingService guideBookingService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
            _photographerBookingService = photographerBookingService;
            _guideBookingService = guideBookingService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Photographer(Guid id)
        {
            var model = new PhotographerBooking()
            {
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture),
                PhotographerId = id,
            };

            //if (await _photographerBookingService.IsBooked(model))
            //{
            //    TempData["Message"] = "This photographer is already booked for this time slot. Please choose another time slot.";
            //    return RedirectToAction("/Photographer/List");
            //}
            _sessionService.SetString("PhotographerId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { string.Empty });
            }
            return RedirectToAction("Photographer", "Review");
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
                    BookingStatus = BookingStatus.Pending
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
            var model = new GuideBooking()
            {
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate")),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime")),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour")),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime")),
                GuideId = id,
            };

            //if (await _guideBookingService.IsBooked(model) == true)
            //{
            //    TempData["Message"] = "This guide is already booked for this time slot. Please choose another time slot.";
            //    return Redirect("/Guide/List");
            //}
            _sessionService.SetString("GuideId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new { area = string.Empty });
            }
            return RedirectToAction("Guide", "Review");
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
                    //guideBooking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                guideBooking.GuideId = Guid.Parse(_sessionService.GetString("GuideId"));
                _guideBookingService.AddBooking(guideBooking,booking);
                return View("Success");
            }
            return View(model);
        }
    }
}
