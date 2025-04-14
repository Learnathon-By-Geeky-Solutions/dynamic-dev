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
    public class AdminBusBookingService: IAdminBusBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public AdminBusBookingService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

  


         public IEnumerable<BusBooking> GetAllBusBookings()
        {
            return _applicationUnitOfWork.BusBookingRepository.GetAllBusBookings();
        }
    }
}
