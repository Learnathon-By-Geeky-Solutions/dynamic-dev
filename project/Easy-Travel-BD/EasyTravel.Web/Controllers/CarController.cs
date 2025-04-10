using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace EasyTravel.Web.Controllers
{
    public class CarController : Controller
    {

        private readonly ICarService _carService;
        private readonly ISessionService _sessionService;
        public CarController(ICarService carService,ISessionService sessionService)
        {

            _carService = carService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return RedirectToAction("Index", "CarSearch");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }


        [HttpGet]
        public async Task<IActionResult> List()
        {

            // Retrieve search parameters from session
            var from = _sessionService.GetString("From");
            var to = _sessionService.GetString("To");
            var dateTime = DateTime.Parse(_sessionService.GetString("DateTime"));

            // Create the model to pass to the view
            var model = new SearchCarResultViewModel
            {
                CarSearchFormModel = new CarSearchFormModel
                {
                    From = from,
                    To = to,
                    DepartureTime = dateTime
                }
            };

            // Get the list of available buses using the BusService
            model.Cars = await _carService.GetAvailableCarsAsync(from, to, dateTime);

            return View(model);
        }

        public IActionResult CarBooking(Guid CarId)
        {
            var car = _carService.GetCarById(CarId);
            if (car == null) return NotFound();

            var viewModel = new CarBookingViewModel
            {
                Car = car,
                CarId = CarId,
                BookingForm = new BookingForm()
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CarBooking(CarBookingViewModel model)
        {
            var carBooking = new CarBooking
            {
                Id = Guid.NewGuid(),
                CarId = model.CarId,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                TotalAmount = model.BookingForm.TotalAmount,
                BookingDate = DateTime.Now,
            };

            // Adding UserId to the CarBooking if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                carBooking.UserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            }

            _carService.SaveBooking(carBooking, model.CarId);

            return RedirectToAction("CarConfirmBooking");
        }


        [HttpGet]

        public IActionResult CarConfirmBooking()
        {

            return View();
        }


      
    }
}
