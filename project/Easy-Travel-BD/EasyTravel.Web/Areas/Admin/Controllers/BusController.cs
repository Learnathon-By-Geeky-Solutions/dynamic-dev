using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        [HttpPost]
        public IActionResult Create(Bus bus)
        {
            if (ModelState.IsValid)
            {
                _busService.CreateBus(bus);
                return RedirectToAction("Index", "Bus", new { area = "Admin" });

            }
            return View();
        }

    }
}

