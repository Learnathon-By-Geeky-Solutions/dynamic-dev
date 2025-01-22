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
            return View();
        }

        public IActionResult Create(Bus bus)
        {

            _busService.CreateBus(bus);
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            var buses = _busService.GetAllBuses();
            return View(buses);
        }
    }
}
