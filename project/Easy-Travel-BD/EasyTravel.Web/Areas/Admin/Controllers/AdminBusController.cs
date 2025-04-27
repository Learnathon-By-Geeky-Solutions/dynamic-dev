using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin, busManager")]
    public class AdminBusController : Controller
    {
        private readonly IBusService _busService;

        public AdminBusController(IBusService busService)
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
                return RedirectToAction("Index", "AdminBus", new { area = "Admin" });

            }
            return View();
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var buses = _busService.GetAllPagenatedBuses(pageNumber,pageSize);
            return View(buses);
        }

      

        [HttpGet]
        public IActionResult Update(Guid BusId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
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
                return RedirectToAction("Index", "AdminBus", new { area = "Admin" });
            }
            return View(bus);
        }

        [HttpGet]
        public IActionResult Delete(Guid BusId)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
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
                return RedirectToAction("Index", "AdminBus", new { area = "Admin" });
            }
            return View(bus);
        }
    }
}

