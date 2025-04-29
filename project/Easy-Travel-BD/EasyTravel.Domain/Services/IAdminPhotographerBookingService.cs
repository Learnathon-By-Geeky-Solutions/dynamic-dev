using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IAdminPhotographerBookingService : IGetService<PhotographerBooking,Guid>
    {
        void Delete(Guid id);
        Task<PagedResult<PhotographerBooking>> GetPaginatedPhotographerBookingAsync(int pageNumber, int pageSize);
    }
}
