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
    public class BusController : Controller
    {
        private readonly IBusService _busService;
        private readonly ISessionService _sessionService;

        public BusController(IBusService busService, ISessionService sessionService)
        {
            _busService = busService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                return RedirectToAction("Bus", "Search");
            }
            catch (Exception ex)
            {
                // Log the error
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
            model.Buses = await _busService.GetAvailableBusesAsync(from, to, dateTime);

            return View(model);
        }
    
        [HttpGet]
        public IActionResult SelectSeats(Guid busId)
        {
            var bus = _busService.GetseatBusById(busId);
            if (bus == null)
                return NotFound();

            // Ensure seats are populated
            var model = new BusSeatsViewModel
            {
                Bus = bus,
                Seats = bus.Seats
            };

            return View(model);
        }

    }
}
