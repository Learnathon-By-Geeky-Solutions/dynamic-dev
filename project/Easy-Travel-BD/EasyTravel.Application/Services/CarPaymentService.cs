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
    public class CarPaymentService:IPaymentBookingService<CarBooking, Booking, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public CarPaymentService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public Guid AddPayment(CarBooking entity, Booking entity2)
        {
            _applicationUnitOfWork.BookingRepository.Add(entity2);
            _applicationUnitOfWork.Save();
            entity.Id = entity2.Id;
            _applicationUnitOfWork.CarBookingRepository.Add(entity);
            _applicationUnitOfWork.Save();
            return entity.Id;
        }
    }
}
