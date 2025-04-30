using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IHotelBookingService:IService<HotelBooking,Guid>
    {
        void SaveBooking(HotelBooking model, Booking booking, Payment? payment = null);
    }
}
