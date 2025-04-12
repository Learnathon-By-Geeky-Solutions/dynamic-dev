using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using System.Transactions;
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

        public void SaveBooking(CarBooking booking, Guid CarId)
        {
            // Start a transaction to ensure both operations succeed or fail together
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Save the booking
                    _applicationUnitOfWork1.CarBookingRepository.Add(booking);
                    _applicationUnitOfWork1.Save();

                   
                        var car = _applicationUnitOfWork1.CarRepository.GetById(CarId);
                        if (car != null)
                        {
                            car.IsAvailable = false;
                            _applicationUnitOfWork1.CarRepository.Edit(car);
                            _applicationUnitOfWork1.Save();
                        }
                    

                    // Commit the transaction
                    transaction.Complete();
                }
                catch
                {
                    // Transaction will automatically roll back if an exception occurs
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync(string from, string to, DateTime dateTime)
        {
            var cars = await _applicationUnitOfWork1.CarRepository.GetAsync(car =>
                car.From == from &&
                car.To == to &&
                car.DepartureTime.Date == dateTime.Date &&
                car.IsAvailable);

            return cars;
        }






    }
}
