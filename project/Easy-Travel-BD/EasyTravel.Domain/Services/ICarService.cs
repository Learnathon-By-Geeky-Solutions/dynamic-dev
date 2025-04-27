using EasyTravel.Domain.Entites;
using EasyTravel.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Domain.Services
{
    public interface ICarService
    {

        void CreateCar(Car car);
        Task<PagedResult<Car>> GetAllPaginatedCarsAsync(int pageNumber, int pageSize);
        Car GetCarById(Guid CarId);
        void UpdateCar(Car car);
        void DeleteBus(Car car);
        void SaveBooking(CarBooking model,Guid CarId, Booking booking, Payment? payment = null);
        Task<PagedResult<Car>> GetAvailableCarsAsync(string from, string to, DateTime dateTime, int pageNumber, int pageSize);
    }
}
