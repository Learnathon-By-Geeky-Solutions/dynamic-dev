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
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyTravel.Web.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ISessionService _sessionService;
        private readonly IMapper _mapper;
        private readonly IPhotographerService _photographerService;
        private readonly IGuideService _guideService;
        private readonly ICarService _carService;
        private readonly IBookingService _bookingService;
        public ReviewController(ISessionService sessionService, IPhotographerService photographerService,
            UserManager<User> userManager,IMapper mapper, IGuideService guideService, ICarService carService, IBookingService bookingService)
        {
            _sessionService = sessionService;
            _photographerService = photographerService;
            _userManager = userManager;
            _mapper = mapper;
            _guideService = guideService;
            _carService = carService;
            _bookingService = bookingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> PhotographerBooking(Guid id1,Guid id2)
        {
            if (!ModelState.IsValid || Guid.Empty == id1 || Guid.Empty == id2)
            {
                return Redirect("/Photographer/List");
            }
            _sessionService.SetString("LastVisitedPage", "/Review/PhotographerBooking");
            var user = await _userManager.GetUserAsync(User);
            var pgBooking = _mapper.Map<PhotographerBooking>(user);
            var photographer = _photographerService.Get(id2);
            pgBooking.Photographer = photographer;
            pgBooking.EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture);
            pgBooking.StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture);
            pgBooking.EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture);
            pgBooking.TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture);
            pgBooking.EventType = _sessionService.GetString("EventType");
            pgBooking.EventLocation = _sessionService.GetString("EventLocation");
            pgBooking.PhotographerId = photographer.Id;
            pgBooking.Id = id1;
            pgBooking.Booking = _bookingService.Get(id1);
            var bookingmodel = new BookingModel
            {
                Id = id1,
                PhotographerBooking = pgBooking,
                Booking = new Booking
                {
                    TotalAmount = photographer.HourlyRate * pgBooking.TimeInHour,
                }
            };
            return View(bookingmodel);
        }
        [HttpGet]
        public async Task<IActionResult> GuideBooking(Guid id1, Guid id2)
        {
            if (!ModelState.IsValid || Guid.Empty == id1 || Guid.Empty == id2)
            {
                return Redirect("/Guide/List");
            }
            _sessionService.SetString("LastVisitedPage", "/Review/GuideBooking");
            var user = await _userManager.GetUserAsync(User);
            var guideBooking = _mapper.Map<GuideBooking>(user);
            var guide = _guideService.Get(id2);
            guideBooking.Guide = guide;
            guideBooking.EventDate = DateTime.Parse(_sessionService.GetString("EventDate"),CultureInfo.InvariantCulture);
            guideBooking.StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture);
            guideBooking.EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture);
            guideBooking.TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"));
            guideBooking.EventType = _sessionService.GetString("EventType");
            guideBooking.EventLocation = _sessionService.GetString("EventLocation");
            guideBooking.GuideId = guide.Id;
            guideBooking.Id = id1;
            guideBooking.Booking = _bookingService.Get(id1);
            var bookingmodel = new BookingModel
            {
                Id = id1,
                GuideBooking = guideBooking,
                Booking = new Booking
                {
                    TotalAmount = guide.HourlyRate * guideBooking.TimeInHour,
                }
            };
            return View(bookingmodel);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult BusBooking(BusBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _sessionService.SetString("busBookingObj", JsonSerializer.Serialize(model));
            }
            return View(model);
        }

        [Authorize]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CarBooking(CarBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Car = _carService.GetCarById(model.CarId);
                _sessionService.SetString("carBookingObj", JsonSerializer.Serialize(model));
            }
            return View(model);
        }
    }
}
