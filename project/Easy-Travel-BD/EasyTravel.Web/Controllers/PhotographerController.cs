using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Graph;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        private readonly IAgencyService _agencyService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IPhotographerBookingService _photographerBookingService;
        public PhotographerController(IPhotographerService photographerService, UserManager<User> userManager, IMapper mapper, ISessionService sessionService, IAgencyService agencyService, IPhotographerBookingService photographerBookingService)
        {
            _photographerService = photographerService;
            _userManager = userManager;
            _mapper = mapper;
            _sessionService = sessionService;
            _agencyService = agencyService;
            _photographerBookingService = photographerBookingService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Photographer/Index");
            return RedirectToAction("Index", "Search");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Photographer/List");
            var model = new SearchResultViewModel
            {
                SearchFormModel = new SearchFormModel
                {
                    EventDate = DateTime.Parse(_sessionService.GetString("EventDate")),
                    StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime")),
                    TimeInHour = int.Parse(_sessionService.GetString("TimeInHour")),
                    EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime")),
                },
            };
            var pgBookingModel = _mapper.Map<PhotographerBooking>(model.SearchFormModel);
            model.Photographers = await _photographerService.GetPhotographerListAsync(pgBookingModel);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Book(Guid id)
        {
            var model = new PhotographerBooking()
            {
                EventDate = DateTime.Parse(_sessionService.GetString("EventDate")),
                StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime")),
                TimeInHour = int.Parse(_sessionService.GetString("TimeInHour")),
                EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime")),
                PhotographerId = id,
            };

            if (await _photographerBookingService.IsBooked(model) == true)
            {
                TempData["Message"] = "This photographer is already booked for this time slot. Please choose another time slot.";
                return RedirectToAction("/Photographer/List");
            }
            _sessionService.SetString("PhotographerId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("Login", "Account", new {string.Empty});
            }
            return RedirectToAction("Review", "Photographer");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Book(PhotographerBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pgBooking = _mapper.Map<PhotographerBooking>(model);
                if (User.Identity.IsAuthenticated == true)
                {
                    pgBooking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                pgBooking.PhotographerId = Guid.Parse(_sessionService.GetString("PhotographerId"));
                _sessionService.Remove("PhotographerId");
                _photographerBookingService.AddBooking(pgBooking);
                return View();
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Review()
        {
            var pgId = _sessionService.GetString("PhotographerId");
            if (string.IsNullOrEmpty(pgId))
            {
                return Redirect("/Photographer/List");
            }
            _sessionService.SetString("LastVisitedPage", "/Photographer/Review");
            var pgBooking = new PhotographerBookingViewModel();
            var user = await _userManager.GetUserAsync(User);
            pgBooking = _mapper.Map<PhotographerBookingViewModel>(user);
            var photographer = _photographerService.Get(Guid.Parse(pgId));
            pgBooking.Photographer = photographer;
            var agency = _agencyService.Get(photographer.AgencyId);
            pgBooking.AgencyName = agency.Name;
            pgBooking.EventDate = DateTime.Parse(_sessionService.GetString("EventDate"));
            pgBooking.StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"));
            pgBooking.EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"));
            pgBooking.TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"));
            pgBooking.EventType = _sessionService.GetString("EventType");
            pgBooking.EventLocation = _sessionService.GetString("EventLocation");
            pgBooking.TotalAmount = photographer.HourlyRate * pgBooking.TimeInHour;
            return View(pgBooking);
        }
    }
}
