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
            return RedirectToAction("Index","Search");
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
        public IActionResult Book(Guid id)
        {
            _sessionService.SetString("PhotographerId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("BookingForm", "Booking");
            }
            return RedirectToAction("Review", "Photographer");
        }

        [HttpPost,ValidateAntiForgeryToken]
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
            if(string.IsNullOrEmpty(pgId))
            {
                return Redirect(pgId);
            }
            var pgBooking = new PhotographerBookingViewModel();
            if(User.Identity?.IsAuthenticated == false)
            {
                pgBooking.FirstName = _sessionService.GetString("FirstName");
                pgBooking.LastName = _sessionService.GetString("LastName");
                pgBooking.Email = _sessionService.GetString("Email");
                pgBooking.PhoneNumber = _sessionService.GetString("PhoneNumber");
                pgBooking.Gender = _sessionService.GetString("Gender");
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                pgBooking = _mapper.Map<PhotographerBookingViewModel>(user);
            }
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
