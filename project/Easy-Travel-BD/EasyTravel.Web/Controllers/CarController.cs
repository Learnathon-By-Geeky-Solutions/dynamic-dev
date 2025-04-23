using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace EasyTravel.Web.Controllers
{
    public class CarController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ICarService _carService;
        private readonly ISessionService _sessionService;
        public CarController(ICarService carService,ISessionService sessionService, UserManager<User> userManager)
        {

            _carService = carService;
            _sessionService = sessionService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Car/Index");
            return RedirectToAction("Car", "Search");
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
        [HttpGet]
        public async Task<IActionResult> PassengerDetails(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var car = _carService.GetCarById(id);
            if (car == null) return NotFound();

            var viewModel = new CarBookingViewModel
            {
                Car = car,
                CarId = id,
                BookingForm = new BookingForm
                {
                    PassengerName = $"{user.FirstName},{user.LastName}",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    TotalAmount = car.Price
                }
            };
            return View(viewModel);
        }
    }
}
