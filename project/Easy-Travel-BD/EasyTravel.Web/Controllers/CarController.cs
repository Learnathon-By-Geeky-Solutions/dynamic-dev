using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
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

      /*  public IActionResult CarBooking(CarBookingViewModel model)
        {


            var carbooking = new CarBooking
            {
                Id = Guid.NewGuid(),
                BusId = model.BusId,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                TotalAmount = model.TotalAmount, // Using model.TotalAmount directly instead of BookingForm.TotalAmount
                BookingDate = DateTime.Now,
                SelectedSeats = model.SelectedSeatNumbers,
                SelectedSeatIds = model.SelectedSeatIds,


            };

            _busService.SaveBooking(busbooking, model.SelectedSeatIds);
            return RedirectToAction("BusConfirmBooking");








            return View(model);
        }/*


        

        public IActionResult List()
        {
             var cars = _carService.GetAllCars();
            return View(cars);
        }*/
    }
}
