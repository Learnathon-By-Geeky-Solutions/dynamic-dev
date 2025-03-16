using AutoMapper;
using EasyTravel.Domain.Entites;
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
        public UserController(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BookingForm()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/User/BookingForm");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BookingForm(BookingFormViewModel model)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var reguser = await _userManager.GetUserAsync(User);
                model = _mapper.Map<BookingFormViewModel>(reguser);
            }
            if (ModelState.IsValid)
            {
                TempData["MyData"] = JsonSerializer.Serialize(model);
                return RedirectToAction("Book", "Photographer");
            }
            return View(model);
        }
    }
}
