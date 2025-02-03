
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EasyTravel.Web.Controllers
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

            if (ModelState.IsValid)
            {
                _busService.CreateBus(bus);
                return RedirectToAction("List");

            }
            return View();
              
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



        [HttpGet]
        public IActionResult Update(Guid BusId)
        {
            var bus= _busService.GetBusById(BusId);
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
                return RedirectToAction("List");
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
                return RedirectToAction("List");
            }
            return View();

        }


    }
}
