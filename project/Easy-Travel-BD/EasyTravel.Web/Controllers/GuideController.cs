using AutoMapper;
using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class GuideController : Controller
    {
        private readonly IGuideService _guideService;
        private readonly IMapper _mapper;
        private readonly IGuideBookingService _guideBookingService;
        public GuideController(IGuideService guideService,IMapper mapper, IGuideBookingService guideBookingService)
        {
            _guideService = guideService;
            _mapper = mapper;
            _guideBookingService = guideBookingService;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Guide/Index");
            var models = _guideService.GetAll();
            return View(models);
        }

        [HttpGet]
        public IActionResult Book(Guid id)
        {
            if (TempData["MyData"] is null)
            {
                return RedirectToAction("BookingForm", "User");
            }
            var user = JsonSerializer.Deserialize<BookingFormViewModel>((string)TempData["MyData"]);
            var model = _mapper.Map<GuideBookingViewModel>(user);
            var guide = _guideService.Get(id);
            model = _mapper.Map<GuideBookingViewModel>(guide);
            return View(model);
        }

        [HttpPost]
        public IActionResult Book(GuideBookingViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
    }
}
