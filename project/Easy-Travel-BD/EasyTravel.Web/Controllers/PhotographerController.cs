using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public PhotographerController(IPhotographerService photographerService, UserManager<User> userManager, IMapper mapper)
        {
            _photographerService = photographerService;
            _userManager = userManager;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Photographer/Index");
            var models = _photographerService.GetAll();
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
            var model = _mapper.Map<PhotographerBookingViewModel>(user);
            var photographer = _photographerService.Get(id);
            model = _mapper.Map<PhotographerBookingViewModel>(photographer);
            return View(model);
        }

        [HttpPost]
        public IActionResult Book(PhotographerBookingViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }

    }
}
