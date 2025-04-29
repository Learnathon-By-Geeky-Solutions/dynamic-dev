using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminBusBookingService
    {
        IEnumerable<BusBooking> GetAllBusBookings();
        void DeleteBusBooking(Guid Id);
        Task<PagedResult<BusBooking>> GetPaginatedBusBookingsAsync(int pageNumber, int pageSize);
    }


}
