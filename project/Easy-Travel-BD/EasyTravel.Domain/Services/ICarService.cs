using EasyTravel.Domain.Entites;
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
        IEnumerable<Car> GetAllCars();
        Car GetCarById(Guid CarId);
        void UpdateCar(Car car);
        void DeleteBus(Car car);
    }
}
