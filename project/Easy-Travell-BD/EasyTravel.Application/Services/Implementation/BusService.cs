using EasyTravel.Application.Services.Interfaces;
using EasyTravel.Domain.Interfaces;
using EasyTravel.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class BusService : IBusService
    {

        private readonly IBusRepository _busRepository;
        public BusService(IBusRepository busRepository)
        {

            _busRepository = busRepository;
        }
        public void CreateBus(Bus bus)
        {
            _busRepository.Addbus(bus);
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _busRepository.GetAllBuses();
        }
    }
}
