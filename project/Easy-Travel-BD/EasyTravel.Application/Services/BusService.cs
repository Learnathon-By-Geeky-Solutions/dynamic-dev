
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyTravel.Application.Services
{
    public class BusService : IBusService
    {
        private readonly ILogger<BusService> _logger;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public BusService(IApplicationUnitOfWork applicationUnitOfWork, ILogger<BusService> logger)
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;
            _logger = logger;
        }
        public void CreateBus(Bus bus)
        {

            for (char row = 'A'; row <= 'G'; row++)
            {
                for (int col = 1; col <= 4; col++)
                {
                    var seat = new Seat
                    {
                        Id = Guid.NewGuid(),
                        BusId = bus.Id,
                        SeatNumber = $"{row}{col}", 
                        IsAvailable = true
                    };
                    bus.Seats.Add(seat);
                }
            }


            _applicationUnitOfWork1.BusRepository.Addbus(bus);
            _applicationUnitOfWork1.Save();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _applicationUnitOfWork1.BusRepository.GetAllBuses();

        }



        public Bus GetseatBusById(Guid busId)
        {
            // Ensure that the Bus entity and its associated Seats are eagerly loaded
            var bus = _applicationUnitOfWork1.BusRepository
                            .GetBuses()  // Returns IQueryable<Bus>
                            .Include(b => b.Seats)  // Eagerly load Seats
                            .FirstOrDefault(b => b.Id == busId); // Get the bus by busId

            return bus;
        }



        public Bus GetBusById(Guid BusId)
        {
           var bus= _applicationUnitOfWork1.BusRepository.GetById(BusId);

            return bus;
        }

        public async Task<IEnumerable<Bus>> GetAvailableBusesAsync(string from, string to, DateTime dateTime)
        {
            // Get buses that match the search parameters
            var buses = await _applicationUnitOfWork1.BusRepository.GetAsync(bus=>
                bus.From == from &&
                bus.To == to &&
                bus.DepartureTime.Date == dateTime.Date &&
                bus.Seats.Any(seat => seat.IsAvailable));

            return buses;
        }



        public void UpdateBus(Bus bus)
        {
            _applicationUnitOfWork1.BusRepository.Edit(bus);
            _applicationUnitOfWork1.Save();


        }

        public void DeleteBus(Bus bus)
        {
            _applicationUnitOfWork1.BusRepository.Remove(bus);
            _applicationUnitOfWork1.Save();

        }

        public void SaveBooking(BusBooking model,List<Guid> seatIds,Booking booking,Payment? payment = null)
        {
            // Start a transaction to ensure both operations succeed or fail together
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Save the booking
                    _applicationUnitOfWork1.BookingRepository.Add(booking);
                    _applicationUnitOfWork1.Save();
                    _applicationUnitOfWork1.BusBookingRepository.Add(model);
                    _applicationUnitOfWork1.Save();

                    // Update the seat availability
                    foreach (var seatId in seatIds)
                    {
                        var seat = _applicationUnitOfWork1.SeatRepository.GetById(seatId);
                        if (seat != null)
                        {
                            seat.IsAvailable = false;
                            _applicationUnitOfWork1.SeatRepository.Edit(seat);
                            _applicationUnitOfWork1.Save();
                        }
                    }
                    if (payment != null)
                    {
                        _applicationUnitOfWork1.PaymentRepository.Add(payment);
                        _applicationUnitOfWork1.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    // Handle exception (e.g., log it)
                    _logger.LogError(ex, "Error saving booking");
                }
            }
        }





    }
}
