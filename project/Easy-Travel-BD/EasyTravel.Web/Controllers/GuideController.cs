using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly IAgencyService _agencyService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        private readonly IGuideBookingService _guideBookingService;
        public GuideController(IGuideService guideService, UserManager<User> userManager, IMapper mapper, ISessionService sessionService, IAgencyService agencyService, IGuideBookingService guideBookingService)
        {
            _guideService = guideService;
            _userManager = userManager;
            _mapper = mapper;
            _sessionService = sessionService;
            _agencyService = agencyService;
            _guideBookingService = guideBookingService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/Index");
            return RedirectToAction("Index", "Search");
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/List");
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
            var guideBookingModel = _mapper.Map<GuideBooking>(model.SearchFormModel);
            model.Guides = await _guideService.GetGuideListAsync(guideBookingModel);
            return View(model);
        }
        
        [HttpGet]
        public IActionResult Book(Guid id)
        {
            _sessionService.SetString("GuideId", id.ToString());
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("BookingForm", "User");
            }
            return RedirectToAction("Review", "Guide");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Book(GuideBookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var guideBooking = _mapper.Map<GuideBooking>(model);
                if (User.Identity.IsAuthenticated == true)
                {
                    guideBooking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                }
                guideBooking.GuideId = Guid.Parse(_sessionService.GetString("GuideId"));
                _guideBookingService.AddBooking(guideBooking);
                return View();
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Review()
        {
            _sessionService.SetString("LastVisitedPage", "/Guide/Review");
            var guideBooking = new GuideBookingViewModel();
            if (User.Identity?.IsAuthenticated == false)
            {
                guideBooking.FirstName = _sessionService.GetString("FirstName");
                guideBooking.LastName = _sessionService.GetString("LastName");
                guideBooking.Email = _sessionService.GetString("Email");
                guideBooking.PhoneNumber = _sessionService.GetString("PhoneNumber");
                guideBooking.Gender = _sessionService.GetString("Gender");
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                guideBooking = _mapper.Map<GuideBookingViewModel>(user);
            }
            var guideId = Guid.Parse(_sessionService.GetString("GuideId"));
            var guide = _guideService.Get(guideId);
            guideBooking.Guide = guide;
            var agency = _agencyService.Get(guide.AgencyId);
            guideBooking.AgencyName = agency.Name;
            guideBooking.EventDate = DateTime.Parse(_sessionService.GetString("EventDate"));
            guideBooking.StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"));
            guideBooking.EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"));
            guideBooking.TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"));
            guideBooking.EventType = _sessionService.GetString("EventType");
            guideBooking.EventLocation = _sessionService.GetString("EventLocation");
            guideBooking.TotalAmount = guide.HourlyRate * guideBooking.TimeInHour;
            return View(guideBooking);
        }
    }
}
