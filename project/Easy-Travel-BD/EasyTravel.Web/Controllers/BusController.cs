
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Models;
using EasyTravel.Web.ViewModels;
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

        public IActionResult List()
        {
      
            var buses = _busService.GetAllBuses();
            return View(buses);
        }

        [HttpGet]
        public IActionResult SelectSeats(Guid busId)
        {
            var bus = _busService.GetBusById(busId);
            if (bus == null)
                return NotFound();

            return View(bus);
        }
        public IActionResult BusBooking(Guid busId, string selectedSeats, decimal totalAmount)
        {
            

            var bus = _busService.GetBusById(busId);
            if (bus == null) return NotFound();

            var viewModel = new BusBookingViewModel
            {
                Bus = bus,
                BookingForm = new BookingForm(),
                SelectedSeatNumbers = string.IsNullOrEmpty(selectedSeats)
                    ? new List<string>()
                    : selectedSeats.Split(',').ToList(),
                TotalAmount = totalAmount
            };

            return View(viewModel);
        }




    }
}
