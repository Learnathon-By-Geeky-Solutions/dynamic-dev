using EasyTravel.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface IBusService
    {
        void CreateBus(Bus bus);
        IEnumerable<Bus> GetAllBuses();
        Bus GetBusById(Guid BusId);
        void UpdateBus(Bus bus);
        void DeleteBus(Bus bus);
        void SaveBooking(BusBooking model,List<Guid> seatIds, Booking booking, Payment? payment = null);
        Bus GetseatBusById(Guid busId);
        Task<IEnumerable<Bus>> GetAvailableBusesAsync(string from, string to, DateTime dateTime);
    }
}
