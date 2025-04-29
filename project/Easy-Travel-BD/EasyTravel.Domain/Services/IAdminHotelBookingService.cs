using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminHotelBookingService : IGetService<HotelBooking,Guid>
    {
        void Delete(Guid id);
        Task<PagedResult<HotelBooking>> GetPaginatedHotelBookingsAsync(int pageNumber, int pageSize);
    }
}
