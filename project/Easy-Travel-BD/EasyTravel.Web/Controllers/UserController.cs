using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Text.Json;

namespace EasyTravel.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ISessionService _sessionService;
        public UserController(IMapper mapper, UserManager<User> userManager, ISessionService sessionService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BookingForm()
        {
            if(User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Search", "Photographer");
            }
            var lastPage = _sessionService.GetString("LastVisitedPage");
            if(lastPage?.Contains("/Photographer/Index") == true)
            {
                return View();
            }
            return RedirectToAction("Index", "Photographer");
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> BookingForm(BookingFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                _sessionService.SetString("FirstName", model.FirstName);
                _sessionService.SetString("LastName", model.LastName);
                _sessionService.SetString("Email", model.Email);
                _sessionService.SetString("PhoneNumber", model.PhoneNumber);
                _sessionService.SetString("Gender", model.Gender);
                return RedirectToAction("Review", "Photographer");
            }
            return View(model);
        }
    }
}
