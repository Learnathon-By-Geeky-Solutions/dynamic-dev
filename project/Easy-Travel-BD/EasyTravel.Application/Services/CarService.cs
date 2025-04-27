using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace EasyTravel.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ILogger<CarService> _logger;
        private readonly IApplicationUnitOfWork _unitOfWork;

        public CarService(IApplicationUnitOfWork unitOfWork, ILogger<CarService> logger)
        {
            _unitOfWork = unitOfWork ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void CreateCar(Car car)
        {
            if (car == null)
            {
                _logger.LogWarning("Attempted to create a null Car entity.");
                throw new ArgumentNullException(nameof(car), "Car entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Creating a new car with ID: {Id}", car.Id);
                _unitOfWork.CarRepository.AddCar(car);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully created car with ID: {Id}", car.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the car with ID: {Id}", car.Id);
                throw new InvalidOperationException($"An error occurred while creating the car with ID: {car.Id}.", ex);
            }
        }

        public Car GetCarById(Guid CarId)
        {
            if (CarId == Guid.Empty)
            {
                _logger.LogWarning("Invalid car ID provided for retrieval.");
                throw new ArgumentException("Car ID cannot be empty.", nameof(CarId));
            }

            try
            {
                _logger.LogInformation("Fetching car with ID: {Id}", CarId);
                var car = _unitOfWork.CarRepository.GetById(CarId);
                return car;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the car with ID: {Id}", CarId);
                throw new InvalidOperationException($"An error occurred while fetching the car with ID: {CarId}.", ex);
            }
        }

        public void UpdateCar(Car car)
        {
            if (car == null)
            {
                _logger.LogWarning("Attempted to update a null Car entity.");
                throw new ArgumentNullException(nameof(car), "Car entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating car with ID: {Id}", car.Id);
                _unitOfWork.CarRepository.Edit(car);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully updated car with ID: {Id}", car.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the car with ID: {Id}", car.Id);
                throw new InvalidOperationException($"An error occurred while updating the car with ID: {car.Id}.", ex);
            }
        }

        public void DeleteBus(Car car)
        {
            if (car == null)
            {
                _logger.LogWarning("Attempted to delete a null Car entity.");
                throw new ArgumentNullException(nameof(car), "Car entity cannot be null.");
            }

            try
            {
                _logger.LogInformation("Deleting car with ID: {Id}", car.Id);
                _unitOfWork.CarRepository.Remove(car);
                _unitOfWork.Save();
                _logger.LogInformation("Successfully deleted car with ID: {Id}", car.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the car with ID: {Id}", car.Id);
                throw new InvalidOperationException($"An error occurred while deleting the car with ID: {car.Id}.", ex);
            }
        }

        public void SaveBooking(CarBooking model, Guid CarId, Booking booking, Payment? payment = null)
        {
            if (model == null || booking == null)
            {
                _logger.LogWarning("Invalid input provided for saving booking.");
                throw new ArgumentException("Model and booking cannot be null.");
            }

            if (CarId == Guid.Empty)
            {
                _logger.LogWarning("Invalid car ID provided for saving booking.");
                throw new ArgumentException("Car ID cannot be empty.", nameof(CarId));
            }

            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation("Saving booking for car with ID: {CarId}", CarId);

                    // Save the booking
                    _unitOfWork.BookingRepository.Add(booking);
                    _unitOfWork.Save();
                    _unitOfWork.CarBookingRepository.Add(model);
                    _unitOfWork.Save();

                    // Update car availability
                    var car = _unitOfWork.CarRepository.GetById(CarId);
                    if (car != null)
                    {
                        car.IsAvailable = false;
                        _unitOfWork.CarRepository.Edit(car);
                        _unitOfWork.Save();
                    }

                    // Save payment if provided
                    if (payment != null)
                    {
                        _unitOfWork.PaymentRepository.Add(payment);
                        _unitOfWork.Save();
                    }

                    // Commit the transaction
                    transaction.Complete();
                    _logger.LogInformation("Successfully saved booking for car with ID: {CarId}", CarId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while saving the booking for car with ID: {CarId}", CarId);
                    throw new InvalidOperationException($"An error occurred while saving the booking for car with ID: {CarId}.", ex);
                }
            }
        }
        public async Task<PagedResult<Car>> GetAllPaginatedCarsAsync(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Fetching all cars.");

                var totalItems = await _unitOfWork.CarRepository.GetCountAsync();
                var cars = await _unitOfWork.CarRepository.GetAllAsync();
                var paginateCars = cars.
                    OrderBy(c => c.From)
                    .Skip((pageNumber - 1) * pageSize).
                    Take(pageSize)
                    .ToList();
                var result = new PagedResult<Car>
                {
                    Items = paginateCars,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all cars.");
                throw new InvalidOperationException("An error occurred while fetching all cars.", ex);
            }
        }
        public async Task<PagedResult<Car>> GetAvailableCarsAsync(string from, string to, DateTime dateTime,int pageNumber,int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            {
                _logger.LogWarning("Invalid 'from' or 'to' location provided for fetching available cars.");
                throw new ArgumentException("From and To locations cannot be empty.");
            }

            try
            {
                _logger.LogInformation("Fetching available cars from {From} to {To} on {Date}", from, to, dateTime);
                var cars = await _unitOfWork.CarRepository.GetAsync(car =>
                    car.From == from &&
                    car.To == to &&
                    car.DepartureTime.Date == dateTime.Date &&
                    car.IsAvailable);
                var totalItems = cars.Count();
                var paginateCars = cars.
                    OrderBy(c => c.From)
                    .Skip((pageNumber - 1) * pageSize).
                    Take(pageSize)
                    .ToList();
                var result = new PagedResult<Car>
                {
                    Items = paginateCars,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching available cars from {From} to {To} on {Date}", from, to, dateTime);
                throw new InvalidOperationException($"An error occurred while fetching available cars from {from} to {to} on {dateTime}.", ex);
            }
        }

        public async Task<PagedResult<Car>> GetPaginatedCarsAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                throw new ArgumentException("Page number and page size must be greater than zero.");
            }

            try
            {
                _logger.LogInformation("Fetching paginated cars for page {PageNumber} with size {PageSize}.", pageNumber, pageSize);

                // Fetch total count of cars
                var totalItems = await _unitOfWork.CarRepository.GetCountAsync();

                // Fetch paginated cars
                var cars = await _unitOfWork.CarRepository.GetAllAsync();
                var paginatedCars = cars
                    .OrderBy(c => c.OperatorName) // Sort by operator name
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Create the paginated result
                var result = new PagedResult<Car>
                {
                    Items = paginatedCars,
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching paginated cars.");
                throw new InvalidOperationException("An error occurred while fetching paginated cars.", ex);
            }
        }
    }
}



