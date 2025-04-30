using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IGuideBookingService
    {
        bool CancelBooking();
        Task<bool> IsBooked(GuideBooking model);
        public Task<IEnumerable<GuideBooking>> GetBookingListByFormDataAsync(GuideBooking model);
        void SaveBooking(GuideBooking model, Booking booking, Payment? payment = null);
    }
}
