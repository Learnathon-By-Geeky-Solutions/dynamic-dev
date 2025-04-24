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
    public class AdminHotelBookingService: IAdminHotelBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AdminHotelBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public HotelBooking Get(Guid id)
        {
            return _applicationUnitOfWork.HotelBookingRepository.GetById(id);
        }

        public IEnumerable<HotelBooking> GetAll()
        {
            return _applicationUnitOfWork.HotelBookingRepository.GetAll();
        }
        public void Delete(Guid id)
        {
            _applicationUnitOfWork.BookingRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }
    }
}
