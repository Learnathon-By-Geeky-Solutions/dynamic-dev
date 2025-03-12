
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
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

        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public BusService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;

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

        public Bus GetBusById(Guid BusId)
        {
           var bus= _applicationUnitOfWork1.BusRepository.GetById(BusId);

            return bus;
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

        public void SaveBooking(BusBooking booking, List<Guid> seatIds)
        {
            // Start a transaction to ensure both operations succeed or fail together
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Save the booking
                    _applicationUnitOfWork1.BusBookingRepository.Add(booking);
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

                    // Commit the transaction
                    transaction.Complete();
                }
                catch
                {
                    // Transaction will automatically roll back if an exception occurs
                    throw;
                }
            }
        }





    }
}
