using EasyTravel.Application.Services;
using EasyTravel.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Controllers
{
    public class CarController : Controller
    {

        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {

            _carService = carService;

        }

        public IActionResult List()
        {
             var cars = _carService.GetAllCars();
            return View(cars);
        }
    }
}
