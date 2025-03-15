using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
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



        [HttpPost]
        public IActionResult BusBooking(Guid busId, string selectedSeats, string selectedSeatIds, decimal totalAmount)
        {
            var bus = _busService.GetseatBusById(busId);
            if (bus == null) return NotFound();

            var viewModel = new BusBookingViewModel
            {
                BusId=busId,
                Bus = bus,
                BookingForm = new BookingForm(),
                SelectedSeatNumbers = string.IsNullOrEmpty(selectedSeats)
                    ? new List<string>()
                    : selectedSeats.Split(',').ToList(),
                SelectedSeatIds = string.IsNullOrEmpty(selectedSeatIds)
                    ? new List<Guid>()
                    : selectedSeatIds.Split(',').Select(id => Guid.Parse(id)).ToList(),
                TotalAmount = totalAmount
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BusConfirmBooking(BusBookingViewModel model)
        {



            var busbooking = new BusBooking
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
            
        }

        [HttpGet]

        public IActionResult BusConfirmBooking()
        {

            return View();
        }



    




    }
}
