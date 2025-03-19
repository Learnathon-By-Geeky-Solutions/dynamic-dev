using EasyTravel.Application.Services;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
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

        public IActionResult CarBooking(Guid CarId)
        {
            var car = _carService.GetCarById(CarId);
            if (car == null) return NotFound();

            var viewModel = new CarBookingViewModel
            {
                Car = car,
                BookingForm = new BookingForm()
            };

            return View(viewModel);
        }
        

        public IActionResult List()
        {
             var cars = _carService.GetAllCars();
            return View(cars);
        }
    }
}
