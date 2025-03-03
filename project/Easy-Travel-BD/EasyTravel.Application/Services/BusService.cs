
using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Repositories;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
