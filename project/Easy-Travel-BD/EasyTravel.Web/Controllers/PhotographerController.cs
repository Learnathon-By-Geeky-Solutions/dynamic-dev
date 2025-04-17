using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Graph;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
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
        public PhotographerController(IPhotographerService photographerService, UserManager<User> userManager, IMapper mapper, ISessionService sessionService, IAgencyService agencyService)
        {
            _photographerService = photographerService;
            _userManager = userManager;
            _mapper = mapper;
            _sessionService = sessionService;
            _agencyService = agencyService;
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
                    EventDate = DateTime.Parse(_sessionService.GetString("EventDate"), CultureInfo.InvariantCulture),
                    StartTime = TimeSpan.Parse(_sessionService.GetString("StartTime"), CultureInfo.InvariantCulture),
                    TimeInHour = int.Parse(_sessionService.GetString("TimeInHour"), CultureInfo.InvariantCulture),
                    EndTime = TimeSpan.Parse(_sessionService.GetString("EndTime"), CultureInfo.InvariantCulture),
                },
            };
            var pgBookingModel = _mapper.Map<PhotographerBooking>(model.SearchFormModel);
            model.Photographers = await _photographerService.GetPhotographerListAsync(pgBookingModel);
            return View(model);
        }
    }
}
