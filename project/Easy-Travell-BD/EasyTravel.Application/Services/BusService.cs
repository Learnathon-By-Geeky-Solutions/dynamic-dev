
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
            _applicationUnitOfWork1.BusRepository.Addbus(bus);
            _applicationUnitOfWork1.Save();
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return _applicationUnitOfWork1.BusRepository.GetAllBuses();

        }
    }
}
