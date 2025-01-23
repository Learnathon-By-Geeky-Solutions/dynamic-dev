using EasyTravel.Application.Services.Interfaces;
using EasyTravel.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Easy_Travell_BD.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("List");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Create(Bus bus)
        {
            if (HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return RedirectToAction("List");
            }

            _busService.CreateBus(bus);
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            if (HttpContext.Session.GetString("UserLoggedIn") != "true")
            {
                return RedirectToAction("Login", "Account");
            }

            var buses = _busService.GetAllBuses();
            return View(buses);
        }
    }
}
