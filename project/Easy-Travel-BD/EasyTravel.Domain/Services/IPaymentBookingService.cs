using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IPaymentBookingService<TEntity,TEntity2,TKey>
    {
        TKey AddPayment(TEntity entity,TEntity2 entity2);
    }
}
