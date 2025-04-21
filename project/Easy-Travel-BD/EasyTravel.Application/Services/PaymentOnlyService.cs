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
    public class PaymentOnlyService : IPaymentOnlyService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PaymentOnlyService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddPaymentOnly(Payment payment)
        {
            _applicationUnitOfWork.PaymentRepository.Add(payment);
            _applicationUnitOfWork.Save();
        }

        public async Task<bool> IsExist(Guid bookingId)
        {
            return await _applicationUnitOfWork.PaymentRepository.GetAsync(d => d.BookingId == bookingId) != null;
        }
    }
}
