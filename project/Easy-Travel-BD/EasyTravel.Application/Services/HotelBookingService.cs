using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class HotelBookingService : IHotelBookingService
    {
        private readonly IApplicationUnitOfWork _applicationunitOfWork;

        public HotelBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationunitOfWork = applicationUnitOfWork;
        }

        public void Create(HotelBooking entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public HotelBooking Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<HotelBooking> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(HotelBooking entity)
        {
            throw new NotImplementedException();
        }
    }
}
