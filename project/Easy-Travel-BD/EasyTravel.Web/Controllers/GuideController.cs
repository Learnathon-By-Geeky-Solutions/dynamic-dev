using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        public GuideController(IGuideService guideService, IMapper mapper, ISessionService sessionService)
        {
            _guideService = guideService;
            _mapper = mapper;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/Index");
            return RedirectToAction("Guide", "Search");
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1, int pageSize = 10)
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/List");
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
            var guideBookingModel = _mapper.Map<GuideBooking>(model.SearchFormModel);

            // Explicitly cast the result to the correct type
            var result = await _guideService.GetGuideListAsync(guideBookingModel, pageNumber, pageSize);
            model.PageNumber = pageNumber;
            model.Guides = result.Item1;
            model.TotalPages = result.Item2;
            return View(model);
        }
    }
}
