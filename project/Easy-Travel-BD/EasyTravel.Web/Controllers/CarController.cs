﻿using EasyTravel.Application.Services;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

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
                CarId = CarId,
                BookingForm = new BookingForm()
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult CarBooking(CarBookingViewModel model)
        {


            var Carbooking = new CarBooking
            {
                Id = Guid.NewGuid(),
                CarId = model.CarId,
                PassengerName = model.BookingForm.PassengerName,
                Email = model.BookingForm.Email,
                PhoneNumber = model.BookingForm.PhoneNumber,
                TotalAmount = model.BookingForm.TotalAmount, 
                BookingDate = DateTime.Now,

            };

            _carService.SaveBooking(Carbooking, model.CarId);

           
            return RedirectToAction("CarConfirmBooking");
        }

        [HttpGet]

        public IActionResult CarConfirmBooking()
        {

            return View();
        }


        public IActionResult List()
        {
             var cars = _carService.GetAllCars();
            return View(cars);
        }
    }
}
