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
    public class PaymentService : IPaymentService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        public PaymentService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }
        public void AddPayment(Payment payment, Guid trnId, Guid bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
