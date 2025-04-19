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
    public class GuidePaymentService:IPaymentBookingService<GuideBooking,Booking, Guid>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public GuidePaymentService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public Guid AddPayment(GuideBooking entity, Booking entity2)
        {
            _applicationUnitOfWork.BookingRepository.Add(entity2);
            _applicationUnitOfWork.Save();
            entity.Id = entity2.Id;
            _applicationUnitOfWork.GuideBookingRepository.Add(entity);
            _applicationUnitOfWork.Save();
            return entity.Id;
        }
    }
}
