using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingValidationService<TEntity,TKey>
    {
        Task<bool> IsExist(Guid id);
    }
}
