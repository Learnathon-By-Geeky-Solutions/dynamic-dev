using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingHistoryService
    {
        Task<IEnumerable<BusBooking>> GetBusBookingsAsync(Guid id);
        Task<IEnumerable<CarBooking>> GetCarBookingsAsync(Guid id);
        Task<IEnumerable<HotelBooking>> GetHotelBookingsAsync(Guid id);
        Task<IEnumerable<PhotographerBooking>> GetPhotographerBookingsAsync(Guid id);
        Task<IEnumerable<GuideBooking>> GetGuideBookingsAsync(Guid id);
    }
}
