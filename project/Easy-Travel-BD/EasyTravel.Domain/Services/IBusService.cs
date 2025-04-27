using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
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
        Task<PagedResult<Bus>> GetAllPagenatedBuses(int pageNumber, int pageSize);
        Bus GetBusById(Guid BusId);
        void UpdateBus(Bus bus);
        void DeleteBus(Bus bus);
        void SaveBooking(BusBooking model,List<Guid> seatIds, Booking booking, Payment? payment = null);
        Bus GetseatBusById(Guid busId);
        Task<PagedResult<Bus>> GetAvailableBusesAsync(string from, string to, DateTime dateTime, int pageNumber, int pageSize);
    }
}
