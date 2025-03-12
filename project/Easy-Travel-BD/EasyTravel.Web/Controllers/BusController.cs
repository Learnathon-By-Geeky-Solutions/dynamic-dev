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
            var bus = _busService.GetBusById(busId);
            if (bus == null)
                return NotFound();

            return View(bus);
        }

        [HttpPost]
        public IActionResult BusBooking(Guid busId, string selectedSeats, string selectedSeatIds, decimal totalAmount)
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
                BusId = model.Bus.Id,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                TotalAmount = model.BookingForm.TotalAmount,
                BookingDate = DateTime.Now,
                SelectedSeats = model.SelectedSeatNumbers,
                SelectedSeatIds = model.SelectedSeatIds,
                Bus = model.Bus

            };


            _busService.SaveBooking(busbooking, model.SelectedSeatIds);



            return RedirectToAction("BusConfirmBooking", new { id = busbooking.Id });
        }



       /*  public IActionResult BookingConfirmation(Guid busId)
         {
             var booking = _busService.GetBusById(busId);
            if (booking == null)
             {
                 return NotFound();
             }

             return View(booking);
         }*/




    }
}
