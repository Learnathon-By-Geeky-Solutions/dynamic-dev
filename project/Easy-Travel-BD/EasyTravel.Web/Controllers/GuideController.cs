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
    }
}
