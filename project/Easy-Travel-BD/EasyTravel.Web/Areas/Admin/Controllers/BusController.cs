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

        public IActionResult Index()
        {
            var buses = _busService.GetAllBuses();
            return View(buses);
        }

      

        [HttpGet]
        public IActionResult Update(Guid BusId)
        {
            var bus = _busService.GetBusById(BusId);
            if (bus == null)
            {
                return NotFound();
            }


            return View(bus);

        }


        [HttpPost]
        public IActionResult Update(Bus bus)
        {
            if (ModelState.IsValid)
            {
                _busService.UpdateBus(bus);
                return RedirectToAction("Index", "Bus", new { area = "Admin" });
            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(Guid BusId)
        {
            var bus = _busService.GetBusById(BusId);
            if (bus == null)
            {
                return NotFound();
            }


            return View(bus);

        }

        [HttpPost]
        public IActionResult Delete(Bus bus)
        {
            if (ModelState.IsValid)
            {
                _busService.DeleteBus(bus);
                return RedirectToAction("Index", "Bus", new { area = "Admin" });
            }
            return View();

        }


        

    }
}

