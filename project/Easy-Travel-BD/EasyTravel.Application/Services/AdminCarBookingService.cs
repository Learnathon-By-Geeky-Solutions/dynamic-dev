using EasyTravel.Domain;
using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EasyTravel.Application.Services
{
    public class AdminCarBookingService : IAdminCarBookingService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;
        private readonly ILogger<AdminCarBookingService> _logger;

        public AdminCarBookingService(IApplicationUnitOfWork applicationUnitOfWork, ILogger<AdminCarBookingService> logger)
        {
            _applicationUnitOfWork = applicationUnitOfWork ;
            _logger = logger;
        }

        public CarBooking Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid car booking ID provided for retrieval.");
                throw new ArgumentException("Car booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Fetching car booking with ID: {Id}", id);
                var carBooking = _applicationUnitOfWork.CarBookingRepository.GetById(id);
                if (carBooking == null)
                {
                    _logger.LogWarning("Car booking with ID: {Id} not found.", id);
                    throw new KeyNotFoundException($"Car booking with ID: {id} not found.");
                }
                return carBooking;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the car booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while fetching the car booking with ID: {id}.", ex);
            }
        }

        public IEnumerable<CarBooking> GetAll()
        {
            try
            {
                _logger.LogInformation("Fetching all car bookings.");
                return _applicationUnitOfWork.CarBookingRepository.GetAll();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all car bookings.");
                throw new InvalidOperationException("An error occurred while fetching all car bookings.", ex);
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid car booking ID provided for deletion.");
                throw new ArgumentException("Car booking ID cannot be empty.", nameof(id));
            }

            try
            {
                _logger.LogInformation("Attempting to delete car booking with ID: {Id}", id);
                _applicationUnitOfWork.BookingRepository.Remove(id);
                _applicationUnitOfWork.Save();
                _logger.LogInformation("Successfully deleted car booking with ID: {Id}", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the car booking with ID: {Id}", id);
                throw new InvalidOperationException($"An error occurred while deleting the car booking with ID: {id}.", ex);
            }
        }
    }
}
