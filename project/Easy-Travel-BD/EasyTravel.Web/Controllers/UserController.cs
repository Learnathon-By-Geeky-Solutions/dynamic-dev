using AutoMapper;
using EasyTravel.Domain.Entites;
using EasyTravel.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EasyTravel.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult BookingForm()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/User/BookingForm");
            //var model = new BookingFormViewModel
            //{
            //    PhotographerId = id,
            //};
            return View();
        }
        [HttpPost]
        public IActionResult BookingForm(BookingFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Book", "Photographer", new { model });
            }
            return View();
        }
    }
}
