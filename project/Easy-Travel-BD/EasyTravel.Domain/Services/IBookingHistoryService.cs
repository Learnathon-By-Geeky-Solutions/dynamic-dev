using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingHistoryService
    {
        Task<PagedResult<BusBooking>> GetBusBookingsAsync(Guid id, int pageNumber, int pageSize);
        Task<PagedResult<CarBooking>> GetCarBookingsAsync(Guid id, int pageNumber, int pageSize);
        Task<PagedResult<HotelBooking>> GetHotelBookingsAsync(Guid id, int pageNumber, int pageSize);
        Task<PagedResult<PhotographerBooking>> GetPhotographerBookingsAsync(Guid id, int pageNumber, int pageSize);
        Task<PagedResult<GuideBooking>> GetGuideBookingsAsync(Guid id, int pageNumber, int pageSize);
    }
}
