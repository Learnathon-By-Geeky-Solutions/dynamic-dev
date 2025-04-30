using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasyTravel.Web.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusService _busService;
        private readonly ISessionService _sessionService;
        private readonly UserManager<User> _userManager;
        public BusController(IBusService busService, ISessionService sessionService, UserManager<User> userManager)
        {
            _busService = busService;
            _sessionService = sessionService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("LastVisitedPage", "/Bus/Index");
            return RedirectToAction("Bus", "Search");
        }

        [HttpGet]
        public async Task<IActionResult> List(string from, string to, string date, int pageNumber = 1, int pageSize = 10)
        {
            // If parameters are not provided, retrieve them from the session
            from ??= _sessionService.GetString("From");
            to ??= _sessionService.GetString("To");
            date ??= _sessionService.GetString("DateTime");

            // Parse the date
            var dateTime = DateTime.Parse(date, CultureInfo.InvariantCulture);

            // Save the search parameters in the session
            _sessionService.SetString("From", from);
            _sessionService.SetString("To", to);
            _sessionService.SetString("DateTime", date);

            // Create the model to pass to the view
            var model = new SearchBusResultViewModel
            {
                busSearchFormModel = new BusSearchFormModel
                {
                    From = from,
                    To = to,
                    DepartureTime = dateTime
                }
            };

            // Get the list of available buses using the BusService
            var result = await _busService.GetAvailableBusesAsync(from, to, dateTime, pageNumber, pageSize);
            model.Buses = result.Item1;
            model.PageNumber = pageNumber;
            model.TotalPages = result.Item2;

            return View(model);
        }



        [HttpGet]
        public IActionResult SelectSeats(Guid id1,Guid id2)
        {
            if(!ModelState.IsValid)
                return View();
            var bus = _busService.GetseatBusById(id2);
            if (bus == null)
                return NotFound();

            // Ensure seats are populated
            var model = new BusSeatsViewModel
            {
                Bus = bus,
                Seats = bus.Seats,
                BookingId = id1
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PassengerDetails(Guid BookingId,Guid busId, string selectedSeats, string selectedSeatIds, decimal totalAmount)
        {
            if (!ModelState.IsValid)
                return View();
            var user = await _userManager.GetUserAsync(User);
            var bus = _busService.GetseatBusById(busId);
            if (bus == null) return NotFound();

            var viewModel = new BusBooking
            {
                Id = BookingId,
                BusId = busId,
                Bus = bus,
                PassengerName = $"{user?.FirstName},{user?.LastName}",
                Email = user?.Email!,
                PhoneNumber = user?.PhoneNumber == null ? "0175621465" : user?.PhoneNumber!,
                SelectedSeats = string.IsNullOrEmpty(selectedSeats)
                    ? new List<string>()
                    : selectedSeats.Split(',').ToList(),
                SelectedSeatIds = string.IsNullOrEmpty(selectedSeatIds)
                    ? new List<Guid>()
                    : selectedSeatIds.Split(',').Select(id => Guid.Parse(id)).ToList(),
            };
            var booking = new BookingModel
            {
                Id = BookingId,
                BusBooking = viewModel,
                Booking = new Booking
                {
                    TotalAmount = totalAmount,
                }
            };
            return View(booking);
        }


    }
}
