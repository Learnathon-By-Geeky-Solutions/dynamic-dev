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
                TempData["success"] = "The bus has created successfully";

                return RedirectToAction("Index", "Bus", new { area = "Admin" });

            }
            TempData["error"] = "The Bus not created ";

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
                TempData["error"] = "The bus not found";

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
                TempData["success"] = "The bus has updated successfully";

                return RedirectToAction("Index", "Bus", new { area = "Admin" });
            }
            TempData["error"] = "The bus not updated";

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
                TempData["success"] = "The bus has deleted successfully";

                _busService.DeleteBus(bus);
                return RedirectToAction("Index", "Bus", new { area = "Admin" });
            }
            TempData["error"] = "The bus not found";

            return View();

        }


        

    }
}

