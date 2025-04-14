using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Repositories
{
    public interface IBusBookingRepository: IRepository<BusBooking, Guid>
    {

        IEnumerable<BusBooking> GetAllBusBookings();
        void DeleteBusBooking(Guid Id);
    }
}
