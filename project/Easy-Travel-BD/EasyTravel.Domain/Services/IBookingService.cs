using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetBookingListAsync(TEntity model);
        void AddBooking(TEntity model);
        bool CancelBooking();
    }
}
