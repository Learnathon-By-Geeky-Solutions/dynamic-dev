using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IPaymentBookingService<TEntity,TEntity2>
    {
        Guid GetBookingId(TEntity entity,TEntity2 entity2);
    }
}
