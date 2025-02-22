using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyTravel.Application.Services
{
    public class CarService : ICarService
    {

        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public CarService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;

        }
        public void CreateCar(Car car)
        {
            _applicationUnitOfWork1.CarRepository.AddCar(car);
            _applicationUnitOfWork1.Save();
        }

        public IEnumerable<Car> GetAllCars()
        {
            return _applicationUnitOfWork1.CarRepository.GetAllCars();
        }
        public Car GetCarById(Guid CarId)
        {
            var car = _applicationUnitOfWork1.CarRepository.GetById(CarId);

            return car;
        }

        public void UpdateCar(Car car)
        {
            _applicationUnitOfWork1.CarRepository.Edit(car);
            _applicationUnitOfWork1.Save();


        }

        public void DeleteBus(Car car)
        {
            _applicationUnitOfWork1.CarRepository.Remove(car);
            _applicationUnitOfWork1.Save();

        }





    }
}
