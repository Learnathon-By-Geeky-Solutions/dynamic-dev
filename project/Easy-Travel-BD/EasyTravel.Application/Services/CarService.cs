using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System.Transactions;
namespace EasyTravel.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly IApplicationUnitOfWork _applicationUnitOfWork1;
        public CarService(IApplicationUnitOfWork applicationUnitOfWork,ILogger<CarService> logger )
        {
            _applicationUnitOfWork1 = applicationUnitOfWork;
            _logger = logger;
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

        public void SaveBooking(CarBooking model, Guid CarId,Booking booking,Payment? payment = null)
        {
            // Start a transaction to ensure both operations succeed or fail together
            using (var transaction = new TransactionScope())
            {
                try
                {
                    // Save the booking
                    _applicationUnitOfWork1.BookingRepository.Add(booking);
                    _applicationUnitOfWork1.Save();
                    _applicationUnitOfWork1.CarBookingRepository.Add(model);
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
                catch (Exception ex)
                {
                    // Handle exception (e.g., log it)
                    _logger.LogError(ex, "Error saving booking");
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
