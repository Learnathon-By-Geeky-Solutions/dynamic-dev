using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IPhotographerBookingService
    {
        PhotographerBooking Get(Guid id);
        bool CancelBooking();
        Task<bool> IsBooked(PhotographerBooking model);
        public Task<IEnumerable<PhotographerBooking>> GetBookingListByFormDataAsync(PhotographerBooking model);
        void SaveBooking(PhotographerBooking model, Booking booking, Payment? payment = null);
    }
}
