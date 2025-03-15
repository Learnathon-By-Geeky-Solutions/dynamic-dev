using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace EasyTravel.Web.Controllers
{
    public class PhotographerController : Controller
    {
        private readonly IPhotographerService _photographerService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public PhotographerController(IPhotographerService photographerService, UserManager<User> userManager,IMapper mapper)
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
        public async Task<IActionResult> Book(Guid id)
        {
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToAction("User", "BookingForm");
            }
            var user = await _userManager.GetUserAsync(User);
            return View();
        }
        [HttpPost]
        public IActionResult Book(BookingFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var pgmodel = _mapper.Map<PhotographerBookingViewModel>(model);
                var photographer = _photographerService.Get(model.PhotographerId);

            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Book(PhotographerBookingViewModel model)
        {
            return View(model);
        }
       
    }
}
