using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "admin,carManager")]
    public class AdminCarController : Controller
    {
        private readonly ICarService _carService;

        public AdminCarController(ICarService carService)
        {
            _carService = carService;
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Car car)
        {
            if (ModelState.IsValid)
            {
                _carService.CreateCar(car);
                return RedirectToAction("Index", "AdminCar", new { area = "Admin" });

            }
            return View();
        }

        public IActionResult Index()
        {
            var cars = _carService.GetAllCars();
            return View(cars);
        }

        [HttpGet]
        public IActionResult Update(Guid CarId)
        {
            if (!ModelState.IsValid)
            {
                //
            }
            var bus = _carService.GetCarById(CarId);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        [HttpPost]
        public IActionResult Update(Car car)
        {
            if (ModelState.IsValid)
            {
                _carService.UpdateCar(car);
                return RedirectToAction("Index", "Admincar", new { area = "Admin" });
            }
            return View();

        }

        [HttpGet]
        public IActionResult Delete(Guid CarId)
        {
            if (ModelState.IsValid)
            {
                //
            }
            var bus = _carService.GetCarById(CarId);
            if (bus == null)
            {
                return NotFound();
            }
            return View(bus);
        }

        [HttpPost]
        public IActionResult Delete(Car car)
        {
            if (ModelState.IsValid)
            {
                _carService.DeleteBus(car);
                return RedirectToAction("Index", "Admincar", new { area = "Admin" });
            }
            return View();

        }




    }
}
