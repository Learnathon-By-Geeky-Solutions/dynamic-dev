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
        Task<IEnumerable<BusBooking>> GetBusBookings(Guid id);
        Task<IEnumerable<CarBooking>> GetCarBookings(Guid id);
        Task<IEnumerable<HotelBooking>> GetHotelBookings(Guid id);
        Task<IEnumerable<PhotographerBooking>> GetPhotographerBookings(Guid id);
        Task<IEnumerable<GuideBooking>> GetGuideBookings(Guid id);
    }
}
