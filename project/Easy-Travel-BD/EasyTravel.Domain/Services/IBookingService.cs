using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBookingService : IGetService<Booking,Guid>
    {
        void AddBooking(Booking booking);
        void RemoveBooking(Guid id);
    }
}
