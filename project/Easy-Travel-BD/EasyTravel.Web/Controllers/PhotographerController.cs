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
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        public PhotographerController(IPhotographerService photographerService, IMapper mapper, ISessionService sessionService)
        {
            _photographerService = photographerService;
            _mapper = mapper;
            _sessionService = sessionService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Photographer/Index");
            return RedirectToAction("Photographer", "Search");
        }
        [HttpGet]
        public async Task<IActionResult> List(int pageNumber = 1,int pageSize = 10)
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
            var result = await _photographerService.GetPhotographerListAsync(pgBookingModel,pageNumber,pageSize);
            model.PageNumber = pageNumber;
            model.Photographers = result.Item1;
            model.TotalPages = result.Item2;
            return View(model);
        }
    }
}
